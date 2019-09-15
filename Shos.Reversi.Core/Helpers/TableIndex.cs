using System;

namespace Shos.Reversi.Core.Helpers
{
    public struct TableIndex
    {
        public enum Direction
        { Left, LeftTop, Top, RightTop, Right, RightBottom, Bottom, LeftBottom }

        public static int MaximumRowNumber    = 8               ;
        public static int MaximumColumnNumber = MaximumRowNumber;

        public static Direction[] AllDirections { get; } = new[] { Direction.Left, Direction.LeftTop, Direction.Top, Direction.RightTop, Direction.Right, Direction.RightBottom, Direction.Bottom, Direction.LeftBottom };

        public int Row   ;
        public int Column;

        public bool IsValid => 0 <= Row && Row < MaximumRowNumber && 0 <= Column && Column < MaximumColumnNumber;

        public int LinearIndex => Row * MaximumColumnNumber + Column;
        public static TableIndex FromLinearIndex(int linearIndex) => new TableIndex { Row = linearIndex / MaximumColumnNumber, Column = linearIndex % MaximumColumnNumber };

        public override bool Equals(object obj) => Row.Equals(((TableIndex)obj).Row) && Column.Equals(((TableIndex)obj).Column);
        public override int GetHashCode() => LinearIndex;

        public static bool operator ==(TableIndex index1, TableIndex index2) => index1.Equals(index2);
        public static bool operator !=(TableIndex index1, TableIndex index2) => !(index1 == index2);

        public bool Forward(Direction direction) => direction switch {
                                                                  Direction.Left        => ToLeft       (),
                                                                  Direction.LeftTop     => ToLeftTop    (),
                                                                  Direction.Top         => ToTop        (),
                                                                  Direction.RightTop    => ToRightTop   (),
                                                                  Direction.Right       => ToRight      (),
                                                                  Direction.RightBottom => ToRightBottom(),
                                                                  Direction.Bottom      => ToBottom     (),
                                                                  Direction.LeftBottom  => ToLeftBottom (),
                                                                  _                     => throw new InvalidOperationException()
                                                              };

        public bool ToTop()
        {
            if (Row <= 0)
                return false;
            Row--;
            return true;
        }

        public bool ToBottom()
        {
            if (Row >= MaximumRowNumber - 1)
                return false;
            Row++;
            return true;
        }

        public bool ToLeft()
        {
            if (Column <= 0)
                return false;
            Column--;
            return true;
        }

        public bool ToRight()
        {
            if (Column >= MaximumColumnNumber - 1)
                return false;
            Column++;
            return true;
        }

        public bool ToLeftTop()
        {
            if (Row <= 0 || Column <= 0)
                return false;
            Row   --;
            Column--;
            return true;
        }

        public bool ToLeftBottom()
        {
            if (Row >= MaximumRowNumber - 1 || Column <= 0)
                return false;
            Row   ++;
            Column--;
            return true;
        }

        public bool ToRightTop()
        {
            if (Row <= 0 || Column >= MaximumColumnNumber - 1)
                return false;
            Row   --;
            Column++;
            return true;
        }

        public bool ToRightBottom()
        {
            if (Row >= MaximumRowNumber - 1 || Column >= MaximumColumnNumber - 1)
                return false;
            Row   ++;
            Column++;
            return true;
        }
    }
}
