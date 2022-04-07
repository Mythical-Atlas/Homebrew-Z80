# Homebrew-Z80
A homebrew computer project using a Zilog Z80 and an Arduino Uno.

<b>Sub-Projects:</b>

- A ROM programmer using a custom-designed PCB and an Arduino Uno. This receives ROM files over a serial connection and writes them to an EEPROM chip.

![pcb image](/PCB.jpg)

- A Windows application to facilitate transferring ROM files to the ROM programmer.

![transfer image](/Transfer.png)

- A Zilog Z80 emulator. This helps with programming and debugging software for the Z80.

![emu image](/Code.png)

- An assembler. This takes a sort of pigeon assembly (based on NASM using Intel's syntax, but not exactly standard-conforming) and converts to the Z80 instruction set.

![assembly image](/Assembly.png)

- A C compiler (work-in-progress). This is possibly the most ambitious and complex part of this project. It's exactly what it sounds like: a compiler that takes C code and converts it to the Z80 instruction set.

![c image](/C Assembly.png)

- A homebrew computer with a Zilog Z80 (work-in-progress). I have designed an early version of the PCB to hold the Z80, but I want to finish the earlier projects before I start ordering and revising PCBs because they're fairly expensive.

I'm not using any guides or tutorials for this project; I am simply using data sheets and other documentation. I am looking at other people's similar projects as examples, but I am not following in their footsteps exactly. The purpose of this project is to gain experience in working with hardware and low-level software.

<b>ROM Programmer PCB and Schematic:</b> https://a360.co/370l1Hq

You don't need an Autodesk account to view it. Click the file folder at the bottom to switch between the PCB and schematic.
