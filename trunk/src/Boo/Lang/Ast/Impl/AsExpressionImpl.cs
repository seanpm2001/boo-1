﻿#region license
// boo - an extensible programming language for the CLI
// Copyright (C) 2004 Rodrigo B. de Oliveira
//
// This program is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation; either version 2
// of the License, or (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
//
// As a special exception, if you link this library with other files to
// produce an executable, this library does not by itself cause the
// resulting executable to be covered by the GNU General Public License.
// This exception does not however invalidate any other reasons why the
// executable file might be covered by the GNU General Public License.
//
// Contact Information
//
// mailto:rbo@acm.org
#endregion

//
// DO NOT EDIT THIS FILE!
//
// This file was generated automatically by
// astgenerator.boo on 2/24/2004 7:16:37 PM
//

namespace Boo.Lang.Ast.Impl
{
	using System;
	using Boo.Lang.Ast;
	
	[Serializable]
	public abstract class AsExpressionImpl : Expression
	{

		protected Expression _target;
		protected TypeReference _type;

		protected AsExpressionImpl()
		{
			InitializeFields();
		}
		
		protected AsExpressionImpl(LexicalInfo info) : base(info)
		{
			InitializeFields();
		}
		

		protected AsExpressionImpl(Expression target, TypeReference type)
		{
			InitializeFields();
			Target = target;
			Type = type;
		}
			
		protected AsExpressionImpl(LexicalInfo lexicalInfo, Expression target, TypeReference type) : base(lexicalInfo)
		{
			InitializeFields();
			Target = target;
			Type = type;
		}
			
		new public Boo.Lang.Ast.AsExpression CloneNode()
		{
			return (Boo.Lang.Ast.AsExpression)Clone();
		}

		override public NodeType NodeType
		{
			get
			{
				return NodeType.AsExpression;
			}
		}
		
		override public void Switch(IAstTransformer transformer, out Node resultingNode)
		{
			Boo.Lang.Ast.AsExpression thisNode = (Boo.Lang.Ast.AsExpression)this;
			Boo.Lang.Ast.Expression resultingTypedNode = thisNode;
			transformer.OnAsExpression(thisNode, ref resultingTypedNode);
			resultingNode = resultingTypedNode;
		}

		override public bool Replace(Node existing, Node newNode)
		{
			if (base.Replace(existing, newNode))
			{
				return true;
			}

			if (_target == existing)
			{
				this.Target = (Boo.Lang.Ast.Expression)newNode;
				return true;
			}

			if (_type == existing)
			{
				this.Type = (Boo.Lang.Ast.TypeReference)newNode;
				return true;
			}

			return false;
		}

		override public object Clone()
		{
			Boo.Lang.Ast.AsExpression clone = (Boo.Lang.Ast.AsExpression)System.Runtime.Serialization.FormatterServices.GetUninitializedObject(typeof(Boo.Lang.Ast.AsExpression));
			clone._lexicalInfo = _lexicalInfo;
			clone._documentation = _documentation;
			clone._properties = (System.Collections.Hashtable)_properties.Clone();
			

			if (null != _target)
			{
				clone._target = (Expression)_target.Clone();
			}

			if (null != _type)
			{
				clone._type = (TypeReference)_type.Clone();
			}
			
			return clone;
		}
			
		public Expression Target
		{
			get
			{
				return _target;
			}
			

			set
			{
				if (_target != value)
				{
					_target = value;
					if (null != _target)
					{
						_target.InitializeParent(this);

					}
				}
			}
			

		}
		

		public TypeReference Type
		{
			get
			{
				return _type;
			}
			

			set
			{
				if (_type != value)
				{
					_type = value;
					if (null != _type)
					{
						_type.InitializeParent(this);

					}
				}
			}
			

		}
		

		private void InitializeFields()
		{

		}
	}
}
