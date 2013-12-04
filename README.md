TFS Power Tools Wrapper
=========================

This is a hack to fix that tfpt.exe annotate does not output date and username.
The idea is to act as a wrapper for tfpt.exe and transform the output. 
The date and username are retrieved using the "tf.exe history" command.

Just put this tfpt.exe before the original in the path. The location of the original tfpt.exe is currently hardcoded to "C:\Program Files (x86)\Microsoft Team Foundation Server 2010 Power Tools\TFPT.exe"


