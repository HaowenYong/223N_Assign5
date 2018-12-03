rm .*dll
rm .*exe

ls -l

mcs -target:library -r:System.Drawing.dll -r:System.Windows.Forms.dll -out:AppleTree.dll AppleTree.cs

mcs -r:System -r:System.Windows.Forms -r:AppleTree.dll -out:tree.exe AppleTreeMain.cs

./tree.exe