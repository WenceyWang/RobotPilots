# Serial Port Transmission Standard

Wencey Wang (wenceywang@dreamry.org)

## Abstract

We have plenty type of package.

There are two different way to serialize a collection of data: XML and Binary.

##Data Package

###XML

The XML way work like this

```xml
<TargetPosition X="234.4565" Y="418.1999" Z="251.303" Id="902466346"/>
```


###Binary

1 byte Magic Number as Header: -86 (1010_1010)

1 byte Sequence

1 byte Package Type

1 byte CRC of Data

1 byte Length of Data

Length byte of Data







