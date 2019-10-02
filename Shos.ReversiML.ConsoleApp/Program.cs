// This file was auto-generated by ML.NET Model Builder. 

using System;
using System.IO;
using System.Linq;
using Microsoft.ML;
using Shos_ReversiML.Model;

namespace Shos_ReversiML.ConsoleApp
{
    class Program
    {
        //Dataset to use for predictions 
        private const string DATA_FILEPATH = @"G:\Dropbox\Source\GitHub\Repos\Shos.Reversi.Data\Shos.Reversi.00011.csv";

        static void Main(string[] args)
        {
            // Create single instance of sample data from first line of dataset for model input
            ModelInput sampleData = CreateSingleDataSample(DATA_FILEPATH);

            // Make a single prediction on the sample data and print results
            ModelOutput predictionResult = ConsumeModel.Predict(sampleData);

            Console.WriteLine("Using model to make single prediction -- Comparing actual Victory with predicted Victory from sample data...\n\n");
            Console.WriteLine($"Cell00: {sampleData.Cell00}");
            Console.WriteLine($"Cell01: {sampleData.Cell01}");
            Console.WriteLine($"Cell02: {sampleData.Cell02}");
            Console.WriteLine($"Cell03: {sampleData.Cell03}");
            Console.WriteLine($"Cell04: {sampleData.Cell04}");
            Console.WriteLine($"Cell05: {sampleData.Cell05}");
            Console.WriteLine($"Cell06: {sampleData.Cell06}");
            Console.WriteLine($"Cell07: {sampleData.Cell07}");
            Console.WriteLine($"Cell10: {sampleData.Cell10}");
            Console.WriteLine($"Cell11: {sampleData.Cell11}");
            Console.WriteLine($"Cell12: {sampleData.Cell12}");
            Console.WriteLine($"Cell13: {sampleData.Cell13}");
            Console.WriteLine($"Cell14: {sampleData.Cell14}");
            Console.WriteLine($"Cell15: {sampleData.Cell15}");
            Console.WriteLine($"Cell16: {sampleData.Cell16}");
            Console.WriteLine($"Cell17: {sampleData.Cell17}");
            Console.WriteLine($"Cell20: {sampleData.Cell20}");
            Console.WriteLine($"Cell21: {sampleData.Cell21}");
            Console.WriteLine($"Cell22: {sampleData.Cell22}");
            Console.WriteLine($"Cell23: {sampleData.Cell23}");
            Console.WriteLine($"Cell24: {sampleData.Cell24}");
            Console.WriteLine($"Cell25: {sampleData.Cell25}");
            Console.WriteLine($"Cell26: {sampleData.Cell26}");
            Console.WriteLine($"Cell27: {sampleData.Cell27}");
            Console.WriteLine($"Cell30: {sampleData.Cell30}");
            Console.WriteLine($"Cell31: {sampleData.Cell31}");
            Console.WriteLine($"Cell32: {sampleData.Cell32}");
            Console.WriteLine($"Cell33: {sampleData.Cell33}");
            Console.WriteLine($"Cell34: {sampleData.Cell34}");
            Console.WriteLine($"Cell35: {sampleData.Cell35}");
            Console.WriteLine($"Cell36: {sampleData.Cell36}");
            Console.WriteLine($"Cell37: {sampleData.Cell37}");
            Console.WriteLine($"Cell40: {sampleData.Cell40}");
            Console.WriteLine($"Cell41: {sampleData.Cell41}");
            Console.WriteLine($"Cell42: {sampleData.Cell42}");
            Console.WriteLine($"Cell43: {sampleData.Cell43}");
            Console.WriteLine($"Cell44: {sampleData.Cell44}");
            Console.WriteLine($"Cell45: {sampleData.Cell45}");
            Console.WriteLine($"Cell46: {sampleData.Cell46}");
            Console.WriteLine($"Cell47: {sampleData.Cell47}");
            Console.WriteLine($"Cell50: {sampleData.Cell50}");
            Console.WriteLine($"Cell51: {sampleData.Cell51}");
            Console.WriteLine($"Cell52: {sampleData.Cell52}");
            Console.WriteLine($"Cell53: {sampleData.Cell53}");
            Console.WriteLine($"Cell54: {sampleData.Cell54}");
            Console.WriteLine($"Cell55: {sampleData.Cell55}");
            Console.WriteLine($"Cell56: {sampleData.Cell56}");
            Console.WriteLine($"Cell57: {sampleData.Cell57}");
            Console.WriteLine($"Cell60: {sampleData.Cell60}");
            Console.WriteLine($"Cell61: {sampleData.Cell61}");
            Console.WriteLine($"Cell62: {sampleData.Cell62}");
            Console.WriteLine($"Cell63: {sampleData.Cell63}");
            Console.WriteLine($"Cell64: {sampleData.Cell64}");
            Console.WriteLine($"Cell65: {sampleData.Cell65}");
            Console.WriteLine($"Cell66: {sampleData.Cell66}");
            Console.WriteLine($"Cell67: {sampleData.Cell67}");
            Console.WriteLine($"Cell70: {sampleData.Cell70}");
            Console.WriteLine($"Cell71: {sampleData.Cell71}");
            Console.WriteLine($"Cell72: {sampleData.Cell72}");
            Console.WriteLine($"Cell73: {sampleData.Cell73}");
            Console.WriteLine($"Cell74: {sampleData.Cell74}");
            Console.WriteLine($"Cell75: {sampleData.Cell75}");
            Console.WriteLine($"Cell76: {sampleData.Cell76}");
            Console.WriteLine($"Cell77: {sampleData.Cell77}");
            Console.WriteLine($"\n\nActual Victory: {sampleData.Victory} \nPredicted Victory value {predictionResult.Prediction} \nPredicted Victory scores: [{String.Join(",", predictionResult.Score)}]\n\n");
            Console.WriteLine("=============== End of process, hit any key to finish ===============");
            Console.ReadKey();
        }

        // Change this code to create your own sample data
        #region CreateSingleDataSample
        // Method to load single row of dataset to try a single prediction
        private static ModelInput CreateSingleDataSample(string dataFilePath)
        {
            // Create MLContext
            MLContext mlContext = new MLContext();

            // Load dataset
            IDataView dataView = mlContext.Data.LoadFromTextFile<ModelInput>(
                                            path: dataFilePath,
                                            hasHeader: true,
                                            separatorChar: ',',
                                            allowQuoting: true,
                                            allowSparse: false);

            // Use first line of dataset as model input
            // You can replace this with new test data (hardcoded or from end-user application)
            ModelInput sampleForPrediction = mlContext.Data.CreateEnumerable<ModelInput>(dataView, false)
                                                                        .First();
            return sampleForPrediction;
        }
        #endregion
    }
}
