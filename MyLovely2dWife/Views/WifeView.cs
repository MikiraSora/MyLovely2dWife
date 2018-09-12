using L2DLib.Framework;
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

        #endregion Winapi

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

        #endregion 변수

        #region 속성

        public bool IsPrepared { get; set; } = false;

        #endregion 속성

        #region 내부 함수

        private bool Prepare()
        {
            if (!IsPrepared && Model != null)
            {
                _armL = new L2DProperty(Model, "PARAM_ARM_L");
                _armR = new L2DProperty(Model, "PARAM_ARM_R");

                _bodyX = new L2DProperty(Model, "PARAM_BODY_ANGLE_X");
                _bodyZ = new L2DProperty(Model, "PARAM_BODY_ANGLE_Z");

                _angleX = new L2DProperty(Model, "PARAM_ANGLE_X");
                _angleY = new L2DProperty(Model, "PARAM_ANGLE_Y");
                _angleZ = new L2DProperty(Model, "PARAM_ANGLE_Z");

                _mouthOpen = new L2DProperty(Model, "PARAM_MOUTH_OPEN_Y");

                _eyeX = new L2DProperty(Model, "PARAM_EYE_BALL_X");
                _eyeY = new L2DProperty(Model, "PARAM_EYE_BALL_Y");

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

        #endregion 내부 함수

        private const float WIDTH = (1920 / 2.0f);
        private const float HEIGHT = (1080 / 2.0f);

        private const float X_OFFSET = -0.5f; //从右边看整个屏幕(屙屎全屏
        private const float Y_OFFSET = 0f;

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
                        var x = (point.X - WIDTH) / WIDTH;
                        var y = -(point.Y - HEIGHT) / HEIGHT;

                        x = MathHelper.Clamp(x + X_OFFSET, -1, 1);
                        y = MathHelper.Clamp(y + Y_OFFSET, -1, 1);

                        var eye_anime_len = 0.75f;
                        _eyeX.AnimatableValue = fast_clamp(x, eye_anime_len);
                        _eyeY.AnimatableValue = fast_clamp(y, eye_anime_len);
                        /*
                        var arm_anime_len = 1;
                        _armL.AnimatableValue = MathHelper.Clamp(arm_anime_len * x, -arm_anime_len, arm_anime_len);
                        _armR.AnimatableValue = MathHelper.Clamp(arm_anime_len * y, -arm_anime_len, arm_anime_len);
                        */
                        var angle_anime_len = 30;
                        _angleX.AnimatableValue = fast_clamp(x, angle_anime_len);
                        _angleY.AnimatableValue = fast_clamp(y, angle_anime_len);

                        var body_anime_len = 5;
                        _bodyX.AnimatableValue = fast_clamp(x, body_anime_len);
                        _bodyZ.AnimatableValue = fast_clamp(y, body_anime_len);

                        _mouthOpen.AnimatableValue = 0.5f;

                        //Console.WriteLine($"point:{point} see:({_eyeX.AnimatableValue},{_eyeY.AnimatableValue})");
                    }
                }

                Model.SaveParam();
            }

            float fast_clamp(float val, float len) => MathHelper.Clamp(len * val, -len, len);
        }
    }
}