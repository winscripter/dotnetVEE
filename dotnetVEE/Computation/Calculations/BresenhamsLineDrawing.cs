namespace dotnetVEE.Computation.Calculations
{
    internal static class BresenhamsLineDrawing
    {
        public static List<(int, int)> GetLineCoordinates(int x1, int y1, int x2, int y2)
        {
            List<(int, int)> coordinates = new List<(int, int)>();

            int dx = Math.Abs(x2 - x1);
            int dy = Math.Abs(y2 - y1);
            int sx = x1 < x2 ? 1 : -1;
            int sy = y1 < y2 ? 1 : -1;
            int err = dx - dy;
            int x = x1;
            int y = y1;

            while (true)
            {
                coordinates.Add((x, y));

                if (x == x2 && y == y2)
                {
                    break;
                }

                int e2 = err * 2;

                if (e2 > -dy)
                {
                    err -= dy;
                    x += sx;
                }

                if (e2 < dx)
                {
                    err += dx;
                    y += sy;
                }
            }

            return coordinates;
        }
    }
}
