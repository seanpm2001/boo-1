﻿#region license
// boo - an extensible programming language for the CLI
// Copyright (C) 2004 Rodrigo Barreto de Oliveira
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
// 
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
// 
// Contact Information
//
// mailto:rbo@acm.org
#endregion

namespace Boo.Lang.Compiler.TypeSystem
{	
	public class ExternalField : IField
	{
		TypeSystemServices _typeSystemServices;
		
		System.Reflection.FieldInfo _field;
		
		public ExternalField(TypeSystemServices tagManager, System.Reflection.FieldInfo field)
		{
			_typeSystemServices = tagManager;
			_field = field;
		}
		
		public IType DeclaringType
		{
			get
			{
				return _typeSystemServices.Map(_field.DeclaringType);
			}
		}
		
		public string Name
		{
			get
			{
				return _field.Name;
			}
		}
		
		public string FullName
		{
			get
			{
				return _field.DeclaringType.FullName + "." + _field.Name;
			}
		}
		
		public bool IsPublic
		{
			get
			{
				return _field.IsPublic;
			}
		}
		
		public bool IsStatic
		{
			get
			{
				return _field.IsStatic;
			}
		}
		
		public bool IsLiteral
		{
			get
			{
				return _field.IsLiteral;
			}
		}
		
		public EntityType EntityType
		{
			get
			{
				return EntityType.Field;
			}
		}
		
		public IType Type
		{
			get
			{
				return _typeSystemServices.Map(_field.FieldType);
			}
		}
		
		public object StaticValue
		{
			get
			{
				return _field.GetValue(null);
			}
		}
		
		public System.Reflection.FieldInfo FieldInfo
		{
			get
			{
				return _field;
			}
		}
	}
}
