"""
def foo(value as int):
	raise System.ArgumentException('value') unless (value > 3)
"""
def foo([required(value > 3)] value as int):
	pass
