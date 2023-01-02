using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeDesigner.UI.Node.Blocks.Types
{
    public enum BinOp
    {
        #region Arithmetic
        Add,
        Subtract,
        Multiply,
        Divide,
        Modulo,
        #endregion

        #region Logical
        LogicalAnd,
        LogicalOr,
        LogicalNot,
        #endregion

        #region Comparison
        Equal,
        NotEqual,
        LessThan,
        LessThanOrEqual,
        GreaterThan,
        GreaterThanOrEqual,
        #endregion

        #region Bitwise
        And,
        Or,
        Xor,
        Invert,
        LeftShift,
        RightShift,
        #endregion

        #region Assignment
        Assign,
        AddAssign,
        SubtractAssign,
        MultiplyAssign,
        DivideAssign
        #endregion
    }
}
