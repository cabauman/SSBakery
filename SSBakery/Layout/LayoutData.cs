using Xamarin.Forms;

namespace SSBakery.UI.Layout
{
    public struct LayoutData
    {
        public LayoutData(int visibleChildCount, Size cellSize, int rows, int columns)
            : this()
        {
            VisibleChildCount = visibleChildCount;
            CellSize = cellSize;
            Rows = rows;
            Columns = columns;
        }

        public int VisibleChildCount { get; private set; }

        public Size CellSize { get; private set; }

        public int Rows { get; private set; }

        public int Columns { get; private set; }
    }
}
