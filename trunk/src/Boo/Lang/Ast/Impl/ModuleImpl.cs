#region license
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
// This file was generated automatically by the
// ast.py script on Tue Feb 10 19:00:39 2004
//
using System;

namespace Boo.Lang.Ast.Impl
{
	[Serializable]
	public abstract class ModuleImpl : TypeDefinition
	{
		protected NamespaceDeclaration _namespace;
		protected ImportCollection _imports;
		protected Block _globals;
		
		protected ModuleImpl()
		{
			_imports = new ImportCollection(this);
			Globals = new Block();
 		}
		
		protected ModuleImpl(NamespaceDeclaration namespace_)
		{
			_imports = new ImportCollection(this);
			Globals = new Block();
 			Namespace = namespace_;
		}
		
		protected ModuleImpl(LexicalInfo lexicalInfo, NamespaceDeclaration namespace_) : base(lexicalInfo)
		{
			_imports = new ImportCollection(this);
			Globals = new Block();
 			Namespace = namespace_;				
		}
		
		protected ModuleImpl(LexicalInfo lexicalInfo) : base(lexicalInfo)
		{
			_imports = new ImportCollection(this);
			Globals = new Block();
 		}
		
		public override NodeType NodeType
		{
			get
			{
				return NodeType.Module;
			}
		}
		public NamespaceDeclaration Namespace
		{
			get
			{
				return _namespace;
			}
			
			set
			{
				
				if (_namespace != value)
				{
					_namespace = value;
					if (null != _namespace)
					{
						_namespace.InitializeParent(this);
					}
				}
			}
		}
		public ImportCollection Imports
		{
			get
			{
				return _imports;
			}
			
			set
			{
				
				if (_imports != value)
				{
					_imports = value;
					if (null != _imports)
					{
						_imports.InitializeParent(this);
					}
				}
			}
		}
		public Block Globals
		{
			get
			{
				return _globals;
			}
			
			set
			{
				
				if (_globals != value)
				{
					_globals = value;
					if (null != _globals)
					{
						_globals.InitializeParent(this);
					}
				}
			}
		}
		new public Module CloneNode()
		{
			return (Module)Clone();
		}
		
		override public object Clone()
		{
			Module clone = (Module)System.Runtime.Serialization.FormatterServices.GetUninitializedObject(GetType());
			clone._lexicalInfo = _lexicalInfo;
			clone._documentation = _documentation;
			clone._properties = (System.Collections.Hashtable)_properties.Clone();
			
			if (null != _namespace)
			{
				clone._namespace = (NamespaceDeclaration)_namespace.Clone();
			}
			if (null != _imports)
			{
				clone._imports = (ImportCollection)_imports.Clone();
			}
			if (null != _globals)
			{
				clone._globals = (Block)_globals.Clone();
			}
			clone._modifiers = _modifiers;
			clone._name = _name;
			if (null != _attributes)
			{
				clone._attributes = (AttributeCollection)_attributes.Clone();
			}
			if (null != _members)
			{
				clone._members = (TypeMemberCollection)_members.Clone();
			}
			if (null != _baseTypes)
			{
				clone._baseTypes = (TypeReferenceCollection)_baseTypes.Clone();
			}
			
			return clone;
		}
		
		override public bool Replace(Node existing, Node newNode)
		{
			if (base.Replace(existing, newNode))
			{
				return true;
			}
			
			if (_namespace == existing)
			{
				this.Namespace = (NamespaceDeclaration)newNode;
				return true;
			}
			if (_imports != null)
			{
				Import item = existing as Import;
				if (null != item)
				{
					if (_imports.Replace(item, (Import)newNode))
					{
						return true;
					}
				}
			}
			if (_globals == existing)
			{
				this.Globals = (Block)newNode;
				return true;
			}
			if (_attributes != null)
			{
				Attribute item = existing as Attribute;
				if (null != item)
				{
					if (_attributes.Replace(item, (Attribute)newNode))
					{
						return true;
					}
				}
			}
			if (_members != null)
			{
				TypeMember item = existing as TypeMember;
				if (null != item)
				{
					if (_members.Replace(item, (TypeMember)newNode))
					{
						return true;
					}
				}
			}
			if (_baseTypes != null)
			{
				TypeReference item = existing as TypeReference;
				if (null != item)
				{
					if (_baseTypes.Replace(item, (TypeReference)newNode))
					{
						return true;
					}
				}
			}
			return false;
		}
		
		override public void Switch(IAstTransformer transformer, out Node resultingNode)
		{
			Module thisNode = (Module)this;
			Module resultingTypedNode = thisNode;
			transformer.OnModule(thisNode, ref resultingTypedNode);
			resultingNode = resultingTypedNode;
		}
	}
}
