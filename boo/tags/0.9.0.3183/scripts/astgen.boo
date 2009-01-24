﻿#region license
// Copyright (c) 2004, Rodrigo B. de Oliveira (rbo@acm.org)
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without modification,
// are permitted provided that the following conditions are met:
//
//     * Redistributions of source code must retain the above copyright notice,
//     this list of conditions and the following disclaimer.
//     * Redistributions in binary form must reproduce the above copyright notice,
//     this list of conditions and the following disclaimer in the documentation
//     and/or other materials provided with the distribution.
//     * Neither the name of Rodrigo B. de Oliveira nor the names of its
//     contributors may be used to endorse or promote products derived from this
//     software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
// WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE
// FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
// DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
// SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
// CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
// OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
// THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
#endregion

import System
import System.IO
import Boo.Lang.Compiler
import Boo.Lang.Compiler.Pipelines
import Boo.Lang.Compiler.Ast
import Useful.BooTemplate from Boo.Lang.Useful

class Model:

	_module as Module

	def constructor([required] module as Module):
		self._module = module

	Members:
		get:
			return _module.Members

	def GetConcreteAstNodes():
		for member as TypeDefinition in Members:
			yield member if IsConcreteAstNode(member)

	def GetEnums():
		for member as TypeDefinition in Members:
			yield member if IsEnum(member)
			
	def IsNodeField(field as Field):
		if field.Type.ToString() == "Node": return true
		fieldType = ResolveFieldType(field)
		return fieldType is not null and not IsEnum(fieldType)

	def IsConcreteAstNode(member as TypeMember):
		return not (IsCollection(member) or IsEnum(member) or IsAbstract(member))

	def IsCollection(node as TypeMember):
		return node.Attributes.Contains("collection")

	def IsCollectionField(field as Field):
		return (field.Type as SimpleTypeReference).Name.EndsWith("Collection")

	def IsEnum(node as TypeMember):
		return NodeType.EnumDefinition == node.NodeType

	def IsAbstract(member as TypeMember):
		return member.IsModifierSet(TypeMemberModifiers.Abstract)

	def GetSimpleFields(node as ClassDefinition):
		return array(
			field
			for field as Field in node.Members
			unless IsCollectionField(field)
			or field.Attributes.Contains("auto"))

	def GetAllFields(node as ClassDefinition):
		fields = []
		for item as TypeDefinition in GetTypeHierarchy(node):
			fields.Extend(item.Members)
		return array(Field, fields)

	def IsVisitableField(field as Field):
		type = ResolveFieldType(field)
		return type is not null and not IsEnum(type)

	def GetVisitableFields(item as ClassDefinition):
		fields = []
		for item as TypeDefinition in GetTypeHierarchy(item):
			for field as Field in item.Members:
				fields.Add(field) if IsVisitableField(field)
		return array(Field, fields)

	def IsExpression(node as ClassDefinition):
		return IsSubclassOf(node, "Expression")

	def GetResultingTransformerNode(node as ClassDefinition):
		for subclass in ("Statement", "Expression", "TypeReference"):
			if IsSubclassOf(node, subclass):
				return subclass
		return node.Name

	def IsSubclassOf(node as ClassDefinition, typename as string) as bool:
		for typeref as SimpleTypeReference in node.BaseTypes:
			if typename == typeref.Name:
				return true
			baseType = node.DeclaringType.Members[typeref.Name]
			if baseType and IsSubclassOf(baseType, typename):
				return true
		return false

	def GetCollectionItemType(node as ClassDefinition):
		attribute = node.Attributes.Get("collection")[0]
		reference as ReferenceExpression = attribute.Arguments[0]
		return reference.Name

	def ResolveFieldType(field as Field):
		return _module.Members[(field.Type as SimpleTypeReference).Name]


class CodeTemplate(AbstractTemplate):

	static header = (
"""${read('notice.txt')}
//
// DO NOT EDIT THIS FILE!
//
// This file was generated automatically by astgen.boo.
//
""")

	[property(model)]
	_model as Model

	[property(node)]
	_node as TypeDefinition

	def GetPrivateName(field as Field):
		name = field.Name
		return "_" + name[0:1].ToLower() + name[1:]

	def FormatParameterList(fields):
		return join(
			"${field.Type} ${GetParameterName(field)}"
			for field as Field in fields,
			", ")

	def GetParameterName(field as Field):
		name = field.Name
		name = name[0:1].ToLower() + name[1:]
		if name in ("namespace", "operator"):
			name += "_"
		return name

def read(fname as string):
	using reader=File.OpenText(fname):
		return reader.ReadToEnd()

def GetPath(fname as string):
	return Path.Combine("src/Boo.Lang.Compiler/Ast", fname)

def GetTypeHierarchy(item as ClassDefinition):
	types = []
	ProcessTypeHierarchy(types, item)
	return types

def ProcessTypeHierarchy(types as List, item as ClassDefinition):
	module as Module = item.ParentNode
	for baseTypeRef as SimpleTypeReference in item.BaseTypes:
		if baseType = module.Members[baseTypeRef.Name]:
			ProcessTypeHierarchy(types, baseType)
	types.Add(item)

def parse(fname):
	compiler = BooCompiler()
	compiler.Parameters.Pipeline = Parse()
	compiler.Parameters.Input.Add(Boo.Lang.Compiler.IO.FileInput(fname))
	result = compiler.Run()
	assert 0 == len(result.Errors), result.Errors.ToString()
	return result.CompileUnit.Modules[0]

def loadTemplate(model, fname as string):
	compiler = TemplateCompiler(TemplateBaseClass: CodeTemplate)
	compiler.DefaultImports.Add("Boo.Lang.Compiler.Ast")
	result = compiler.CompileFile(Path.Combine("scripts/Templates", fname))
	assert 0 == len(result.Errors), result.Errors.ToString()

	templateType = result.GeneratedAssembly.GetType("Template")
	template as CodeTemplate = templateType()
	template.model = model
	return template

def applyTemplate(node as TypeDefinition,
			template as CodeTemplate,
			targetFile as string,
			overwriteExistingFile as bool):

	fname = GetPath(targetFile)
	if not overwriteExistingFile:
		return if File.Exists(fname) or File.Exists(fname.Replace(".Generated", ""))

	print targetFile
	using writer=StreamWriter(fname):
		template.node = node
		template.Output = writer
		template.Execute()

def applyModelTemplate(model as Model, templateName as string, targetFile as string, overwriteExistingFile as bool):
	applyTemplate(null, loadTemplate(model, templateName), targetFile, overwriteExistingFile)

start = date.Now

model = Model(parse("ast.model.boo"))
applyModelTemplate(model, "IAstVisitor.cs", "IAstVisitor.Generated.cs", true)
applyModelTemplate(model, "DepthFirstVisitor.cs", "Impl/DepthFirstVisitor.cs", true)
applyModelTemplate(model, "DepthFirstTransformer.cs", "Impl/DepthFirstTransformer.cs", true)
applyModelTemplate(model, "CodeSerializer.cs", "Impl/CodeSerializer.cs", true)
applyModelTemplate(model, "NodeType.cs", "NodeType.Generated.cs", true)

enumTemplate = loadTemplate(model, "Enum.cs")
collectionTemplate = loadTemplate(model, "Collection.cs")
collectionImplTemplate = loadTemplate(model, "CollectionImpl.cs")
nodeTemplate = loadTemplate(model, "Node.cs")
nodeImplTemplate = loadTemplate(model, "NodeImpl.cs")

for member in model.Members:
	continue if member.Attributes.Contains("ignore")

	if model.IsEnum(member):
		applyTemplate(member, enumTemplate, "${member.Name}.Generated.cs", true)
	elif model.IsCollection(member):
		applyTemplate(member, collectionTemplate, "${member.Name}.Generated.cs", false)
		applyTemplate(member, collectionImplTemplate, "Impl/${member.Name}Impl.cs", true)
	else:
		applyTemplate(member, nodeTemplate, "${member.Name}.Generated.cs", false)
		applyTemplate(member, nodeImplTemplate, "Impl/${member.Name}Impl.cs", true)

stop = date.Now
print "ast classes generated in ${stop-start}."

