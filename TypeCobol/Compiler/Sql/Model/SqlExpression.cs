﻿using System;
using System.Diagnostics;
using System.IO;
using TypeCobol.Compiler.CodeElements;
using TypeCobol.Compiler.Scanner;

namespace TypeCobol.Compiler.Sql.Model
{
    public enum SqlExpressionType
    {
        Variable,
        ColumnName,
        Constant
    }
    public abstract class SqlExpression : SqlObject
    {
        public abstract SqlExpressionType ExpressionType { get; }
    }

    public enum SqlConstantType
    {
        Null,
        Integer,
        FloatingPoint,
        Decimal,
        DecimalFloatingPoint,
        CharacterString,
        BinaryString,
        GraphicString,
        Datetime
    }

    public class SqlConstant : SqlExpression
    {
        public SqlConstant(Token literal)
        {
            Literal = literal;
        }

        public override SqlExpressionType ExpressionType => SqlExpressionType.Constant;
        public virtual SqlConstantType ConstantType
        {
            get
            {
                Debug.Assert(Literal.TokenType == TokenType.SQL_NULL ||
                             Literal.TokenType == TokenType.IntegerLiteral ||
                             Literal.TokenType == TokenType.FloatingPointLiteral ||
                             Literal.TokenType == TokenType.DecimalLiteral ||
                             Literal.TokenType == TokenType.SQL_DecimalFloatingPointLiteral ||
                             Literal.TokenType == TokenType.AlphanumericLiteral || Literal.TokenType == TokenType.HexadecimalAlphanumericLiteral ||
                             Literal.TokenType == TokenType.SQL_BinaryStringLiteral ||
                             Literal.TokenType == TokenType.SQL_GraphicStringLiteral);
                switch (Literal.TokenType)
                {
                    case TokenType.SQL_NULL:
                        return SqlConstantType.Null;
                    case TokenType.IntegerLiteral:
                        return SqlConstantType.Integer;
                    case TokenType.FloatingPointLiteral:
                        return SqlConstantType.FloatingPoint;
                    case TokenType.DecimalLiteral:
                        return SqlConstantType.Decimal;
                    case TokenType.SQL_DecimalFloatingPointLiteral:
                        return SqlConstantType.DecimalFloatingPoint;
                    case TokenType.AlphanumericLiteral:
                    case TokenType.HexadecimalAlphanumericLiteral:
                        return SqlConstantType.CharacterString;
                    case TokenType.SQL_BinaryStringLiteral:
                        return SqlConstantType.BinaryString;
                    case TokenType.SQL_GraphicStringLiteral:
                        return SqlConstantType.GraphicString;
                    default:
                        throw new InvalidOperationException($"Unexpected literal token type '{Literal.TokenType}' for SQL Constant.");
                }
            }
        }

        public Token Literal { get; }

        protected override void DumpContent(TextWriter output, int indentLevel)
        {
            DumpProperty(output, nameof(Literal.LiteralValue), Literal.LiteralValue, indentLevel);
        }
        protected override bool VisitSqlObject(ISqlVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }

    public enum DatetimeConstantKind
    {
        Date,
        Time,
        Timestamp
    }

    public class DatetimeConstant : SqlConstant
    {
        public DatetimeConstant(Token literal, DatetimeConstantKind kind, Token tokenKind) : base(literal)
        {
            TokenKind = tokenKind;
            Kind = kind;
        }

        public override SqlConstantType ConstantType => SqlConstantType.Datetime;

        public DatetimeConstantKind Kind { get; }

        public Token TokenKind { get; }

        protected override bool VisitSqlObject(ISqlVisitor visitor)
        {
            return base.VisitSqlObject(visitor) && visitor.Visit(this);
        }
    }

    public class SqlColumnName : SqlExpression
    {
        public SqlColumnName(SymbolReference symbol)
        {
            this.Symbol = symbol;
        }
        public override SqlExpressionType ExpressionType => SqlExpressionType.ColumnName;
        public SymbolReference Symbol { get; }

        protected override void DumpContent(TextWriter output, int indentLevel)
        {
            DumpProperty(output, nameof(Symbol), Symbol, indentLevel);
        }

        protected override bool VisitSqlObject(ISqlVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
}
