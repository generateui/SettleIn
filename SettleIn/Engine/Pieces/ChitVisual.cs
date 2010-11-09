using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.IO;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Media3D;

using SettleInCommon.Board;
using SettleInCommon.Board.Hexes;
using SettleIn.Engine.Boards;
using SettleIn.Engine.Pieces;

namespace SettleIn
{
    /// <summary>
    /// Represents a chit on the board. A chit can be a number from 2-6, 8-12, or random. 
    /// The ChitNumber enum speecifies this. 
    /// </summary>
    [Serializable]
    public class ChitVisual : Piece, IMoveable
    {
        private TranslateTransform3D _Move = new TranslateTransform3D();
        private EChitNumber _Number;
        [NonSerialized]
        [XmlIgnore]
        private ImageBrush _Brush;
        [NonSerialized]
        [XmlIgnore]
        private MeshGeometry3D chit = new MeshGeometry3D();

        public ChitVisual(Point2D point, EChitNumber n)
            : base(point)
        {
            this._Number = n;
            Init();
        }

        private void Init()
        {
            // size of the chit
            int s = 6;
            
            //some offsets to position te chit correctly
            double offsetx = (Hex.Height / 2) - s;
            double offsetz = Hex.HalfWidth - (s / 2);
            
            // Z- order should be slightly above 0, since the chit should be
            // laid on top of the board.
            double offsety = 0.1;
            
            double x = _Point.X;
            double y = _Point.Y;

            Point3D[] pos = new Point3D[4] 
            {
                new Point3D(x + offsetx + 1, offsety, y + offsetz + 1),
                new Point3D(x + offsetx + 2 + s, offsety, y + offsetz + 1),
                new Point3D(x + offsetx + 2 + s, offsety, y + offsetz + 2 + s),
                new Point3D(x + offsetx + 1, offsety, y + offsetz + 2 + s),
            };
            foreach (Point3D p in pos)
            {
                chit.Positions.Add(p);
            }
            Int32[] indices = 
            {
                0,2,1,
                0,3,2,
            };
            Int32Collection col = new Int32Collection();
            foreach (Int32 i in indices)
            {
                col.Add(i);
            }
            chit.TriangleIndices = col;
            chit.TextureCoordinates.Add(new System.Windows.Point(offsetx + 1, offsetz + 1));
            chit.TextureCoordinates.Add(new System.Windows.Point(offsetx + 2 + s, offsetz + 1));
            chit.TextureCoordinates.Add(new System.Windows.Point(offsetx + 2 + s, offsetz + 2 + s));
            chit.TextureCoordinates.Add(new System.Windows.Point(offsetx + 1, offsetz + 2 + s));
            SetImage();
        }
        protected void SetImage()
        {
            _Brush = new ImageBrush(Texture);
            //Material m = new DiffuseMaterial(new System.Windows.Media.SolidColorBrush(Color.FromRgb(50, 50, 50)));
            Material m = new DiffuseMaterial(_Brush);
            GeometryModel3D triangleModel = new GeometryModel3D(chit, m);
            this.Content = triangleModel;
        }
        public void Attach(Model3DGroup m)
        {
            m.Children.Add(this.Content);
        }

        /// <summary>
        /// Function to enable UI behaviour where the user can click or hit a button 
        /// to increase the chitnumber.
        /// </summary>
        /// <param name="n">current chitnumber</param>
        /// <returns>next chitnumber in line</returns>
        public static EChitNumber GetNextChit(EChitNumber n)
        {
            switch (n)
            {
                case EChitNumber.N2:  return EChitNumber.N3;
                case EChitNumber.N3:  return EChitNumber.N4;
                case EChitNumber.N4:  return EChitNumber.N5;
                case EChitNumber.N5:  return EChitNumber.N6;
                case EChitNumber.N6:  return EChitNumber.N8;
                case EChitNumber.N8:  return EChitNumber.N9;
                case EChitNumber.N9:  return EChitNumber.N10;
                case EChitNumber.N10: return EChitNumber.N11;
                case EChitNumber.N11: return EChitNumber.N12;
                case EChitNumber.N12: return EChitNumber.N2;
            }
            return EChitNumber.N2;
        }
        /// <summary>
        /// Function to enable UI behaviour where the user can click or hit a button 
        /// to decrease the chitnumber.
        /// </summary>
        /// <param name="n">Current chitnumber</param>
        /// <returns>Previous chitnumber</returns>
        public static EChitNumber GetPreviousChit(EChitNumber n)
        {
            switch (n)
            {
                case EChitNumber.N2:  return EChitNumber.N12;
                case EChitNumber.N3:  return EChitNumber.N2;
                case EChitNumber.N4:  return EChitNumber.N3;
                case EChitNumber.N5:  return EChitNumber.N4;
                case EChitNumber.N6:  return EChitNumber.N5;
                case EChitNumber.N8:  return EChitNumber.N6;
                case EChitNumber.N9:  return EChitNumber.N8;
                case EChitNumber.N10: return EChitNumber.N9;
                case EChitNumber.N11: return EChitNumber.N10;
                case EChitNumber.N12: return EChitNumber.N11;
            }
            return EChitNumber.N4;
        }

        /// <summary>
        /// Grab the appropriate texture from the texture list
        /// </summary>
        private ImageSource Texture
        {
            get
            {
                switch (_Number)
                {
                    case EChitNumber.N2:  return (ImageSource)Core.Instance.Textures["Chit2Texture"];  
                    case EChitNumber.N3:  return (ImageSource)Core.Instance.Textures["Chit3Texture"];  
                    case EChitNumber.N4:  return (ImageSource)Core.Instance.Textures["Chit4Texture"];  
                    case EChitNumber.N5:  return (ImageSource)Core.Instance.Textures["Chit5Texture"];  
                    case EChitNumber.N6:  return (ImageSource)Core.Instance.Textures["Chit6Texture"];  
                    case EChitNumber.N8:  return (ImageSource)Core.Instance.Textures["Chit8Texture"];  
                    case EChitNumber.N9:  return (ImageSource)Core.Instance.Textures["Chit9Texture"];  
                    case EChitNumber.N10: return (ImageSource)Core.Instance.Textures["Chit10Texture"]; 
                    case EChitNumber.N11: return (ImageSource)Core.Instance.Textures["Chit11Texture"]; 
                    case EChitNumber.N12: return (ImageSource)Core.Instance.Textures["Chit12Texture"];
                    case EChitNumber.Random: return (ImageSource)Core.Instance.Textures["ChitRandomTexture"];
                }
                return null;
            }
        }
        /// <summary>
        /// The number of the chit
        /// </summary>
        public EChitNumber Number
        {
            get { return _Number; }
            set 
            {
                _Number = value;
                SetImage();
            }
        }



        #region IMoveable Members

        public TranslateTransform3D Move
        {
            get { return _Move; }
        }

        #endregion
    }
}
