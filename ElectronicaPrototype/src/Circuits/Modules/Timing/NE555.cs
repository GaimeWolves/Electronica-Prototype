using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Electronica.Base;
using Electronica.Graphics.Output;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XMLSchemes;

namespace Electronica.Circuits.Modules
{
    public sealed class NE555 : Module
    {
        public NE555() : base()
        {
            mModuleSettings = Main.Instance.Content.Load<ModuleSettings>("modules/timing/NE555/settings");
            mModel = Main.Instance.Content.Load<Model>("modules/timing/NE555/model");
            mOffsetTranslation = Matrix.CreateTranslation(mModuleSettings.offset);
            mPositionTranslation = Matrix.Identity;
        }

        public NE555(Vector2 position, float rotation)
        {
            mModuleSettings = Main.Instance.Content.Load<ModuleSettings>("modules/timing/NE555/settings");
            mModel = Main.Instance.Content.Load<Model>("modules/timing/NE555/model");
            mOffsetTranslation = Matrix.CreateTranslation(mModuleSettings.offset);
            mPositionTranslation = Matrix.CreateRotationY(rotation) * Matrix.CreateTranslation(new Vector3(position, 0));
        }
    }
}
