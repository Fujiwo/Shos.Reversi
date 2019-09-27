using Microsoft.ML;
using System.Collections.Generic;
using System.IO;

namespace Shos.Reversi.ML
{
    using Shos.Reversi.Core;
    using Shos.Reversi.Core.Helpers;
    using Shos.ReversiML.Model.DataModels;

    public class MLPlayer : ComputerPlayer
    {
        MLContext                                  mlContext = new MLContext();
        PredictionEngine<ModelInput, ModelOutput>? predictionEngine;

        public MLPlayer(string modelPath)
        {
            ITransformer mlModel = mlContext.Model.Load(GetAbsolutePath(modelPath), out DataViewSchema inputSchema);
            predictionEngine     = mlContext.Model.CreatePredictionEngine<ModelInput, ModelOutput>(mlModel);
        }

        protected override void Reset()
        {
            predictionEngine?.Dispose();
            predictionEngine = null;
        }

        protected override TableIndex OnTurn(Board board, Stone.StoneState myState, IEnumerable<TableIndex> indexes)
        {
            var bestPrediction = (index: new TableIndex(), prediction: float.MinValue);
            indexes.ForEach(index => {
                var temporaryBoard = board.Clone();
                temporaryBoard.TurnOverWith(index, myState);
                var inputData = CreateInput(temporaryBoard, myState);
                var outputData = predictionEngine.Predict(inputData);
                if (outputData.Prediction > bestPrediction.prediction)
                    bestPrediction = (index, outputData.Prediction);
            });
            return bestPrediction.index;
        }

        ModelInput CreateInput(Board board, Stone.StoneState myState)
            => new ModelInput {
                Cell00 = ToCell(myState, board, 0, 0),
                Cell01 = ToCell(myState, board, 0, 1),
                Cell02 = ToCell(myState, board, 0, 2),
                Cell03 = ToCell(myState, board, 0, 3),
                Cell04 = ToCell(myState, board, 0, 4),
                Cell05 = ToCell(myState, board, 0, 5),
                Cell06 = ToCell(myState, board, 0, 6),
                Cell07 = ToCell(myState, board, 0, 7),
                Cell10 = ToCell(myState, board, 1, 0),
                Cell11 = ToCell(myState, board, 1, 1),
                Cell12 = ToCell(myState, board, 1, 2),
                Cell13 = ToCell(myState, board, 1, 3),
                Cell14 = ToCell(myState, board, 1, 4),
                Cell15 = ToCell(myState, board, 1, 5),
                Cell16 = ToCell(myState, board, 1, 6),
                Cell17 = ToCell(myState, board, 1, 7),
                Cell20 = ToCell(myState, board, 2, 0),
                Cell21 = ToCell(myState, board, 2, 1),
                Cell22 = ToCell(myState, board, 2, 2),
                Cell23 = ToCell(myState, board, 2, 3),
                Cell24 = ToCell(myState, board, 2, 4),
                Cell25 = ToCell(myState, board, 2, 5),
                Cell26 = ToCell(myState, board, 2, 6),
                Cell27 = ToCell(myState, board, 2, 7),
                Cell30 = ToCell(myState, board, 3, 0),
                Cell31 = ToCell(myState, board, 3, 1),
                Cell32 = ToCell(myState, board, 3, 2),
                Cell33 = ToCell(myState, board, 3, 3),
                Cell34 = ToCell(myState, board, 3, 4),
                Cell35 = ToCell(myState, board, 3, 5),
                Cell36 = ToCell(myState, board, 3, 6),
                Cell37 = ToCell(myState, board, 3, 7),
                Cell40 = ToCell(myState, board, 4, 0),
                Cell41 = ToCell(myState, board, 4, 1),
                Cell42 = ToCell(myState, board, 4, 2),
                Cell43 = ToCell(myState, board, 4, 3),
                Cell44 = ToCell(myState, board, 4, 4),
                Cell45 = ToCell(myState, board, 4, 5),
                Cell46 = ToCell(myState, board, 4, 6),
                Cell47 = ToCell(myState, board, 4, 7),
                Cell50 = ToCell(myState, board, 5, 0),
                Cell51 = ToCell(myState, board, 5, 1),
                Cell52 = ToCell(myState, board, 5, 2),
                Cell53 = ToCell(myState, board, 5, 3),
                Cell54 = ToCell(myState, board, 5, 4),
                Cell55 = ToCell(myState, board, 5, 5),
                Cell56 = ToCell(myState, board, 5, 6),
                Cell57 = ToCell(myState, board, 5, 7),
                Cell60 = ToCell(myState, board, 6, 0),
                Cell61 = ToCell(myState, board, 6, 1),
                Cell62 = ToCell(myState, board, 6, 2),
                Cell63 = ToCell(myState, board, 6, 3),
                Cell64 = ToCell(myState, board, 6, 4),
                Cell65 = ToCell(myState, board, 6, 5),
                Cell66 = ToCell(myState, board, 6, 6),
                Cell67 = ToCell(myState, board, 6, 7),
                Cell70 = ToCell(myState, board, 7, 0),
                Cell71 = ToCell(myState, board, 7, 1),
                Cell72 = ToCell(myState, board, 7, 2),
                Cell73 = ToCell(myState, board, 7, 3),
                Cell74 = ToCell(myState, board, 7, 4),
                Cell75 = ToCell(myState, board, 7, 5),
                Cell76 = ToCell(myState, board, 7, 6),
                Cell77 = ToCell(myState, board, 7, 7),
                //Index = index.LinearIndex
            };

        static float ToCell(Stone.StoneState myState, Board board, int row, int column)
            => ToFloat(myState, board[new TableIndex { Row = row, Column = column }].State);

        static float ToFloat(Stone.StoneState myState, Stone.StoneState state)
            => state switch {
                Stone.StoneState.None   => 0.5f,
                var x when x == myState => 1.0f,
                _                       => 0.0f
            };

        static string GetAbsolutePath(string relativePath)
        {
            var root               = new FileInfo(typeof(Player).Assembly.Location);
            var assemblyFolderPath = root.Directory.FullName;
            var fullPath           = Path.Combine(assemblyFolderPath, relativePath);
            return fullPath;
        }
    }
}
