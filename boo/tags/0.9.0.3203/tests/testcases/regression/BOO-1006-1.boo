"""
0.420 0.4200 42.00 %
0x00ffff 6.5535e+004
"""

f = 0.40
g = 0.02
print "${f+g:f3} ${0.42:f4} ${f+g:p}"

c = 65535
print "0x${c:x6} ${c:e4}"

