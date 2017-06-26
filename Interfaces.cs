
namespace Keeper
{
    namespace Interfaces
    {
        class IMouseMoveColor
        {
            private System.Windows.Forms.Control IObject = null;
            private System.Drawing.Color ActiveColor, InActiveColor, ActiveTextColor, InActiveTextColor;

            static private IMouseMoveColor[] Array;

            public static int FollowNewControl(System.Windows.Forms.Control sender, System.Drawing.Color active_color, System.Drawing.Color inactive_color, System.Drawing.Color active_text_color, System.Drawing.Color inactive_text_color)
            {
                if (Array == null)
                {
                    Array = new IMouseMoveColor[0];
                }
                System.Array.Resize(ref Array, Array.Length + 1);
                Array[Array.Length - 1] = new IMouseMoveColor(sender, active_color, inactive_color, active_text_color, inactive_text_color);

                return Array.Length;
            }

            public static void StopFollowingControl(int id)
            {
                Array[id].Stop();
            }

            private void Stop()
            {
                IObject.MouseEnter -= new System.EventHandler(OnMouseEnter);
                IObject.MouseLeave -= new System.EventHandler(OnMouseLeave);
            }

            private IMouseMoveColor(System.Windows.Forms.Control sender, System.Drawing.Color active_color, System.Drawing.Color inactive_color, System.Drawing.Color active_text_color, System.Drawing.Color inactive_text_color)
            {
                IObject = sender;
                ActiveColor = active_color;
                InActiveColor = inactive_color;
                ActiveTextColor = active_text_color;
                InActiveTextColor = inactive_text_color;

                IObject.MouseEnter += new System.EventHandler(OnMouseEnter);
                IObject.MouseLeave += new System.EventHandler(OnMouseLeave);
            }

            private IMouseMoveColor() { }

            private void OnMouseEnter(System.Object sender, System.EventArgs args)
            {
                IObject.BackColor = ActiveColor;
                IObject.ForeColor = ActiveTextColor;
            }
            private void OnMouseLeave(System.Object sender, System.EventArgs args)
            {
                IObject.BackColor = InActiveColor;
                IObject.ForeColor = InActiveTextColor;
            }
        }

        class IDraggable
        {
            static private IDraggable[] Array;

            private System.Windows.Forms.Control IObject;
            private System.Drawing.Point MouseDownPoint, DownPoint;
            private bool IsDown = false;

            private IDraggable(System.Windows.Forms.Control _object)
            {
                IObject = _object;
                AllowDrag();
            }

            private void AllowDrag()
            {
                IObject.MouseDown += new System.Windows.Forms.MouseEventHandler(OnMouseDown);
                IObject.MouseUp += new System.Windows.Forms.MouseEventHandler(OnMouseUp);
                IObject.MouseMove += new System.Windows.Forms.MouseEventHandler(OnMouseMove);
            }

            private void ForbidDrag()
            {
                IObject.MouseDown -= new System.Windows.Forms.MouseEventHandler(OnMouseDown);
                IObject.MouseUp -= new System.Windows.Forms.MouseEventHandler(OnMouseUp);
                IObject.MouseMove -= new System.Windows.Forms.MouseEventHandler(OnMouseMove);
            }

            private void OnMouseDown(System.Object sender, System.Windows.Forms.MouseEventArgs args)
            {
                MouseDownPoint = System.Windows.Forms.Cursor.Position;
                DownPoint = IObject.Location;
                IsDown = true;
            }

            private void OnMouseMove(System.Object sender, System.Windows.Forms.MouseEventArgs args)
            {
                if (IsDown == false) return;
                int dx = System.Windows.Forms.Cursor.Position.X - MouseDownPoint.X;
                int dy = System.Windows.Forms.Cursor.Position.Y - MouseDownPoint.Y;
                IObject.Location = new System.Drawing.Point(DownPoint.X + dx, DownPoint.Y + dy);
            }

            private void OnMouseUp(System.Object sender, System.Windows.Forms.MouseEventArgs args)
            {
                IsDown = false;
            }

            static public int FollowNewItem(System.Windows.Forms.Control sender)
            {
                if (Array == null) Array = new IDraggable[0];
                System.Array.Resize(ref Array, Array.Length + 1);
                Array[Array.Length - 1] = new IDraggable(sender);
                return Array.Length - 1;
            }
        }

        class IResizable
        {
            private System.Windows.Forms.Control IObject;

        }

        class IInstanceCount
        {
            private int curNum;
            private int maxNum;
            private bool IsInitialized = false;

            public IInstanceCount(int max)
            {
                if (IsInitialized == false)
                {
                    maxNum = max;
                    curNum = 0;
                    IsInitialized = true;
                }
                curNum++;
                if (curNum > maxNum) throw new System.Exception();
            }
        }
    }
}
