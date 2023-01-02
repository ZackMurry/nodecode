using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Accessibility;
using CodeDesigner.UI.Node.Blocks;
using CodeDesigner.UI.Node.Blocks.Nodes;
using CodeDesigner.UI.Node.Interaction;
using CodeDesigner.UI.Node.Interaction.Elements;
using CodeDesigner.UI.Utility.Project;
using CodeDesigner.UI.Windows.Resources.Controls.Panels;

namespace CodeDesigner.UI.Node.Canvas
{
    public static class Canvas
    {
        public static CanvasPanel CanvasControl;
        public static PointF MousePosition;
        public static List<BlockBase> Blocks = new();
        public static bool Connecting;
        public static bool IsOverParameter;
        public static bool ElementClickedOn;
        public static Element? ElementClicked;
        public static Parameter? OverParameter;
        public static BlockBase ConnectingBlock;
        public static BlockBase? DraggingBlock;
        public static Dictionary<String, FunctionInformation> FunctionData = new();
        public static NodeMap NodeMap;

        public static void Initialize(CanvasPanel panel)
        {
            CanvasControl = panel;

        }

        public static void AddNode(BlockBase? block)
        {
            Blocks.Add(block);
            CanvasControl.Refresh();
        }

        public static void Pan(PointF delta)
        {
            BlockBase? block = DraggingBlock != null ? DraggingBlock : IsPointInBlock(delta);
            if (block != null)
            {
                DraggingBlock = block;
                MoveBlock(block, delta);
                return;
            }

            float offsetX = (delta.X - MousePosition.X);
            float offsetY = (delta.Y - MousePosition.Y);

            CanvasControl.OffsetX += offsetX;
            CanvasControl.OffsetY += offsetY;

            CanvasControl.Refresh();

            MousePosition = delta;
        }

        public static void ZoomOut(float value)
        {
            CanvasControl.ZoomFactor += value;
            CanvasControl.Refresh();
        }

        public static void ZoomIn(float value)
        {
            if (CanvasControl.ZoomFactor - value < 0.2)
                return;

            CanvasControl.ZoomFactor -= value;
            CanvasControl.Refresh();
        }

        public static void MoveBlock(BlockBase? block, PointF delta)
        {
            block.Coordinates.X += (delta.X - MousePosition.X) / CanvasControl.ZoomFactor;
            block.Coordinates.Y += (delta.Y - MousePosition.Y) / CanvasControl.ZoomFactor;

            MousePosition = delta;

            CanvasControl.Refresh();
        }

        public static BlockBase? IsPointInBlock(PointF testPoint)
        {
            foreach (BlockBase? block in Blocks)
            {
                RectangleF rect = new (block.Coordinates.X * CanvasControl.ZoomFactor, block.Coordinates.Y * CanvasControl.ZoomFactor, block.Properties.Width * CanvasControl.ZoomFactor, block.Properties.Height * CanvasControl.ZoomFactor);

                if (!rect.Contains(testPoint)) continue;

                if (Connecting && block != ConnectingBlock)
                {
                    OverParameter = PointInParameter(block, testPoint);
                    IsOverParameter = true;
                }
                
                return block;
            }

            return null;
        }

        public static Element? IsPointInElement(BlockBase block, PointF testPoint)
        {
            foreach (Element e in block.Elements)
            {
                float x = (block.Coordinates.X + e.Properties.BlockCoordinates.X) * CanvasControl.ZoomFactor;
                float y = (block.Coordinates.Y + e.Properties.BlockCoordinates.Y) * CanvasControl.ZoomFactor;
                float width = e.Properties.Size.Width * CanvasControl.ZoomFactor;
                float height = e.Properties.Size.Height * CanvasControl.ZoomFactor;
                
                RectangleF rect = new(x, y, width, height);

                if (!rect.Contains(testPoint)) continue;

                e.IsClickedOn = true;
                ElementClickedOn = true;
                ElementClicked = e;

                return e;
            }

            return null;
        }

        public static Parameter? PointInParameter(BlockBase block, PointF testPoint)
        {
            foreach (Parameter? param in block.Parameters)
            {
                RectangleF rect = new(param.Coordinates.X * CanvasControl.ZoomFactor, param.Coordinates.Y * CanvasControl.ZoomFactor, 8 * CanvasControl.ZoomFactor, 8 * CanvasControl.ZoomFactor);
                if (!rect.Contains(testPoint)) continue;
                return param;
            }

            return null;
        }

        public static void Interact(Element e)
        {
            switch (e.GetType().Name)
            {
                case "ButtonElement":
                    ((ButtonElement)e).Method.Invoke();
                    break;
                case "TextBoxElement":
                    InteractionHelper.LoadTextBoxConfig((TextBoxElement)e);
                    break;
                case "IconButtonElement":
                    ((IconButtonElement)e).Method.Invoke();
                    break;
            }
        }

        public static void ConnectParameter(BlockBase connectingBlock, Parameter? parameter)
        {
            if (parameter != null)
            {
                connectingBlock.ConnectedParameters.Add(parameter);
                parameter.ReferenceValue = connectingBlock;

                if (connectingBlock.SecondaryConnecting)
                {
                    parameter.SecondaryConnected = true;
                    connectingBlock.SecondaryOutput = parameter.Parent;
                }
                else if (connectingBlock.Connecting)
                {
                    parameter.Connected = true;
                    connectingBlock.Output = parameter.Parent;
                }
                else if (connectingBlock.NextConnecting)
                {
                    connectingBlock.NextBlock = parameter.Parent;
                    parameter.ReferenceValue.InputBlock = connectingBlock;
                    parameter.NextConnected = true;
                }

            }
        }

        public static void DeleteBlock(BlockBase block)
        {
            if (block.NextBlock != null) block.NextBlock.InputBlock = null;
            if (block.InputBlock != null) block.InputBlock.NextBlock = null;
            block.DestroyConnections();
            Blocks.Remove(block);
            CanvasControl.Refresh();
        }

        public static bool IsPointInPolygon(PointF[] polygon, PointF testPoint)
        {
            bool result = false;
            if (polygon == null || polygon.Length == 0) return false;
            int j = polygon.Count() - 1;
            for (int i = 0; i < polygon.Count(); i++)
            {
                if (polygon[i].Y < testPoint.Y && polygon[j].Y >= testPoint.Y || polygon[j].Y < testPoint.Y && polygon[i].Y >= testPoint.Y)
                {
                    if (polygon[i].X + (testPoint.Y - polygon[i].Y) / (polygon[j].Y - polygon[i].Y) * (polygon[j].X - polygon[i].X) < testPoint.X)
                    {
                        result = !result;
                    }
                }
                j = i;
            }
            return result;
        }

        public static void MouseUp()
        {
            DraggingBlock = null;
        }

        public static void HighlightNode(Guid guid)
        {
            foreach (BlockBase b in Blocks)
                b.IsHighlighted = false;
            
            foreach (BlockBase block in Blocks)
            {
                if (block.Id != guid) continue;
                Console.WriteLine("found node");
                block.IsHighlighted = true;

                float x = CanvasControl.Width / 2f - block.Properties.Width /2f - block.Coordinates.X*CanvasControl.ZoomFactor + CanvasControl.OffsetX;
                float y = CanvasControl.Height / 2f - block.Properties.Height / 2f - block.Coordinates.Y*CanvasControl.ZoomFactor + CanvasControl.OffsetY;
                CanvasControl.OffsetX = x;
                CanvasControl.OffsetY = y;
                CanvasControl.Refresh();
                break;
            }
            
        }
    }
}
