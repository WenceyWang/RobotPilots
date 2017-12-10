Serial Port Transmission Standard

Wencey Wang ([wenceywang@dreamry.org](mailto:wenceywang@dreamry.org))

Abstract

Wehave plenty type of package.

Thereare two different way to serialize a collection of data: XML and Binary.

Data Package

XML

TheXML way work like this

<TargetPosition X="234.4565"Y="418.1999"Z="251.303"Id="902466346"/>

Binary

1byte Magic Number as Header: -86 (1010_1010)

1byte Sequence

1byte Package Type

1byte CRC of Data

1byte Length of Data

Lengthbyte of Data

Data Package Type

 

 

 

TargetPosition=0

 

TargetAngle

4 Byte float X Yawin degree

4 Byte float Y Pitchin degree

 

TargetDeltaAngleDatagram

4 Byte float X Yawin degree

4 Byte float Y Pitchin degree

 

CradleHeadPositionDatagram 

4 Byte float X Yawin degree

4 Byte float Y Pitchin degree

 

 