using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using ImageResizer.Configuration;
using ImageResizer.Resizing;

namespace ImageResizer.Plugins.Tint
{
	public class TintPlugin : BuilderExtension, IPlugin, IQuerystringPlugin
	{
		public IEnumerable<string> GetSupportedQuerystringKeys()
		{
			return new[] { "tint" };
		}

		public IPlugin Install(Config c)
		{
			c.Plugins.add_plugin(this);
			return this;
		}

		public bool Uninstall(Config c)
		{
			c.Plugins.remove_plugin(this);
			return true;
		}

		protected override RequestedAction PostCreateImageAttributes(ImageState s)
		{
			if (!String.IsNullOrEmpty(s.settings["tint"]))
			{
				var colour = ColorTranslator.FromHtml("#" + s.settings["tint"]);
				s.copyAttibutes.SetColorMatrix(new ColorMatrix(Tint(colour)));
			}
			return RequestedAction.None;
		}

		static float[][] Tint(Color tint)
		{

			return (new []{
				new float[]{ (float)tint.R / 255, 0,0,0,0},
				new float[]{0, (float)tint.G / 255, 0,0,0},
				new float[]{0,0, (float)tint.B / 255, 0,0},
				new float[]{0,0,0,1,0},
				new float[]{0,0,0,0,1}}
			);

		}
	}
}
