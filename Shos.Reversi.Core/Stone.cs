using Prism.Mvvm;
using System;

namespace Shos.Reversi.Core
{
    public class Stone : BindableBase
    {
        public enum StoneState
        { None, Black, White }

        StoneState state = StoneState.None;

        public StoneState State {
            get => state;
            set => SetProperty(ref state, value);
        }

        bool canTurnOver = false;

        public bool CanTurnOver {
            get => canTurnOver;
            set => SetProperty(ref canTurnOver, value);
        }

        public StoneState ReverseState => GetReverseState(State);

        public void TurnOver() => State = ReverseState;

        public static StoneState GetReverseState(StoneState state)
            => state switch
            {
                StoneState.None  => StoneState.None ,
                StoneState.Black => StoneState.White,
                StoneState.White => StoneState.Black,
                _                => throw new InvalidOperationException()
            };
    }
}
