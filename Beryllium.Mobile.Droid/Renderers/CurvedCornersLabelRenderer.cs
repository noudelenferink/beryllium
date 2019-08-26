
using Beryllium.Mobile.Droid.Renderers;
using Beryllium.Mobile.UI.Controls;
using Xamarin.Forms;

[assembly: ExportRenderer(typeof(CurvedCornersLabel), typeof(CurvedCornersLabelRenderer))]
namespace Beryllium.Mobile.Droid.Renderers
{
   using Android.Graphics.Drawables;
   using Xamarin.Forms;
   using Xamarin.Forms.Platform.Android;
   using Android.Content;
   using System;
   using Android.Util;


   public class CurvedCornersLabelRenderer : LabelRenderer
   {
      public CurvedCornersLabelRenderer(Context context) : base(context)
      {

      }

      private GradientDrawable _gradientBackground;

      protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
      {
         base.OnElementChanged(e);

         var view = (CurvedCornersLabel)Element;
         if (view == null)
            return;

         // creating gradient drawable for the curved background
         _gradientBackground = new GradientDrawable();
         _gradientBackground.SetShape(ShapeType.Rectangle);
         _gradientBackground.SetColor(view.CurvedBackgroundColor.ToAndroid());

         // Thickness of the stroke line
         _gradientBackground.SetStroke(4, view.CurvedBackgroundColor.ToAndroid());

         // Radius for the curves
         _gradientBackground.SetCornerRadius(
             DpToPixels(this.Context,
                 Convert.ToSingle(view.CurvedCornerRadius)));

         // set the background of the label
         Control.SetBackground(_gradientBackground);
         Control.SetPadding(0, 0, 0, 5);
         Control.SetTextSize(ComplexUnitType.Dip, 12);
      }

      /// <summary>
      /// Device Independent Pixels to Actual Pixles conversion
      /// </summary>
      /// <param name="context"></param>
      /// <param name="valueInDp"></param>
      /// <returns></returns>
      public static float DpToPixels(Context context, float valueInDp)
      {
         DisplayMetrics metrics = context.Resources.DisplayMetrics;
         return TypedValue.ApplyDimension(ComplexUnitType.Dip, valueInDp, metrics);
      }
   }
}
