using System.Windows;
using System.Windows.Media;

namespace ColorPicker.Utils {

    static class DrawingContextExtension {
        public static void DrawPolygon(
            this DrawingContext context,
            Brush brush, Pen pen, Point[] points, FillRule fill_rule) {

            StreamGeometry geo = new() {
                FillRule = fill_rule
            };

            using (StreamGeometryContext sgc = geo.Open()) {
                sgc.BeginFigure(points[0], true, true);

                sgc.PolyLineTo([.. points.Skip(1)], true, false);
            }

            geo.Freeze();

            context.DrawGeometry(brush, pen, geo);
        }
    }
}
