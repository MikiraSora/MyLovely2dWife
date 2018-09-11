using L2DLib.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MyLovely2dWife.Views
{
    public class WifeView : L2DView
    {
        #region Winapi

        [DllImport("User32")]
        public extern static void SetCursorPos(int x, int y);
        public struct POINT
        {
            public int X;
            public int Y;
            public POINT(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }

            public override string ToString() => $"({X},{Y})";
        }

        /// <summary>   
        /// 获取鼠标的坐标   
        /// </summary>   
        /// <param name="lpPoint">传址参数，坐标point类型</param>   
        /// <returns>获取成功返回真</returns>   

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool GetCursorPos(out POINT pt);

        #endregion

        #region 변수
        public L2DProperty _armL;
        public L2DProperty _armR;

        public L2DProperty _bodyX;
        public L2DProperty _bodyZ;

        public L2DProperty _angleX;
        public L2DProperty _angleY;
        public L2DProperty _angleZ;

        public L2DProperty _eyeX;
        public L2DProperty _eyeY;
        public L2DProperty _eyeKiraKira;

        public L2DProperty _mouthOpen;

        public List<string> MotionNames
        {
            get { return (List<string>)GetValue(MotionNamesProperty); }
            set { SetValue(MotionNamesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MotionNames.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MotionNamesProperty =
            DependencyProperty.Register("MotionNames", typeof(List<string>), typeof(WifeView), new PropertyMetadata(new List<string>()));
       
        #endregion

        #region 속성

        public bool IsPrepared { get; set; } = false;
        #endregion

        #region 내부 함수
        private bool Prepare()
        {
            if (!IsPrepared && Model != null)
            {
                _armL = new L2DProperty(Model, "PARAM_ARM_L");
                _armR = new L2DProperty(Model, "PARAM_ARM_R");

                _bodyX = new L2DProperty(Model, "PARAM_BODY_X");
                _bodyZ = new L2DProperty(Model, "PARAM_BODY_Z");

                _angleX = new L2DProperty(Model, "PARAM_ANGLE_X");
                _angleY = new L2DProperty(Model, "PARAM_ANGLE_Y");
                _angleZ = new L2DProperty(Model, "PARAM_ANGLE_Z");

                _eyeX = new L2DProperty(Model, "PARAM_EYE_BALL_X");
                _eyeY = new L2DProperty(Model, "PARAM_EYE_BALL_Y");
                _eyeKiraKira = new L2DProperty(Model, "PARAM_EYE_BALL_KIRAKIRA");

                _mouthOpen = new L2DProperty(Model, "PARAM_MOUTH_OPEN_Y");

                MotionNames = Model.Motion.Keys.ToList();

                IsPrepared = true;
            }

            return IsPrepared;
        }

        private Size MeasureText(TextBox target)
        {
            var formatted =
                new FormattedText(target.Text.Substring(0, target.CaretIndex),
                CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                new Typeface(target.FontFamily, target.FontStyle, target.FontWeight, target.FontStretch),
                target.FontSize, target.Foreground);

            return new Size(formatted.Width, formatted.Height);
        }
        #endregion

        public enum PlayStatus
        {
            Idle,
            Motion
        }

        public override void Rendering()
        {
            if (Prepare())
            {
                Model.LoadParam();

                if (!UpdatedMotion)
                {
                    //空闲状态，看鼠标
                    if (GetCursorPos(out var point))
                    {
                        Console.WriteLine(point);
                    }
                }
                
                /*
                {
                    _armL.AnimatableValue = 0.0f;
                    _armR.AnimatableValue = 0.0f;

                    _bodyX.AnimatableValue = 0.0f;
                    _bodyZ.AnimatableValue = 0.0f;

                    _angleX.AnimatableValue = 0.0f;
                    _angleY.AnimatableValue = 0.0f;

                    _eyeX.AnimatableValue = 0.0f;
                    _eyeY.AnimatableValue = 0.0f;
                    _eyeKiraKira.AnimatableValue = 0.0f;
                }
                */

                Model.SaveParam();
            }
        }
    }
}
