"""
A.Method0
BaseClass.Method0

"""
using Boo.Tests.Ast.Compiler from Boo.Tests

class A(BaseClass):
	def Method0():
		print("A.Method0") #overriden method
		super() #base class method
		
b as BaseClass = A()
b.Method0()

