using Emulator.Chip8Commands;
using System.Collections.Generic;

namespace Emulator.Utils
{
    public class Compiler : Chip8
    {
        private Dictionary<ushort, BaseCommand> GameData;

        public override void LoadGame(byte[] gameData)
        {
            LoadGameToMemory(gameData);
            GameData = Compile(gameData);
        }

        public override void ProcessorStep()
        {
            GameData[PC].Execute();
        }

        private Dictionary<ushort, BaseCommand> Compile(byte[] gameData)
        {
            var ret = new Dictionary<ushort, BaseCommand>();

            for (ushort PC = 0; PC < gameData.Length; PC += 2)
            {
                var Opcode = (ushort)(gameData[PC] << 8 | gameData[PC + 1]);

                switch (Opcode & 0xF000)
                {
                    case 0x0000:
                        switch (Opcode & 0x000F)
                        {
                            case 0x0000: // 0x00E0: Clears the screen
                                ret.Add((ushort)(PC + 0x200), new x00E0(this));
                                break;

                            case 0x000E: // 0x00EE: Returns from subroutine
                                ret.Add((ushort)(PC + 0x200), new x00EE(this));
                                break;

                            default:
                                LogError("0000");
                                break;
                        }
                        break;

                    case 0x1000: // 0x1NNN: Jumps to address NNN
                        ret.Add((ushort)(PC + 0x200), new x1NNN(Opcode, this));
                        break;

                    case 0x2000: // 0x2NNN: Calls subroutine at NNN.
                        ret.Add((ushort)(PC + 0x200), new x2NNN(Opcode, this));
                        break;

                    case 0x3000: // 0x3XNN: Skips the next instruction if VX equals NN
                        ret.Add((ushort)(PC + 0x200), new x3XNN(Opcode, this));
                        break;

                    case 0x4000: // 0x4XNN: Skips the next instruction if VX doesn't equal NN
                        ret.Add((ushort)(PC + 0x200), new x4XNN(Opcode, this));
                        break;

                    case 0x5000: // 0x5XY0: Skips the next instruction if VX equals VY.
                        ret.Add((ushort)(PC + 0x200), new x5XY0(Opcode, this));
                        break;

                    case 0x6000: // 0x6XNN: Sets VX to NN.
                        ret.Add((ushort)(PC + 0x200), new x6XNN(Opcode, this));
                        break;

                    case 0x7000: // 0x7XNN: Adds NN to VX.
                        ret.Add((ushort)(PC + 0x200), new x7XNN(Opcode, this));
                        break;

                    case 0x8000:
                        switch (Opcode & 0x000F)
                        {
                            case 0x0000: // 0x8XY0: Sets VX to the value of VY
                                ret.Add((ushort)(PC + 0x200), new x8XY0(Opcode, this));
                                break;

                            case 0x0001: // 0x8XY1: Sets VX to "VX OR VY"
                                ret.Add((ushort)(PC + 0x200), new x8XY1(Opcode, this));
                                break;

                            case 0x0002: // 0x8XY2: Sets VX to "VX AND VY"
                                ret.Add((ushort)(PC + 0x200), new x8XY2(Opcode, this));
                                break;

                            case 0x0003: // 0x8XY3: Sets VX to "VX XOR VY"
                                ret.Add((ushort)(PC + 0x200), new x8XY3(Opcode, this));
                                break;

                            case 0x0004: // 0x8XY4: Adds VY to VX. VF is set to 1 when there's a carry, and to 0 when there isn't					
                                ret.Add((ushort)(PC + 0x200), new x8XY4(Opcode, this));
                                break;

                            case 0x0005: // 0x8XY5: VY is subtracted from VX. VF is set to 0 when there's a borrow, and 1 when there isn't
                                ret.Add((ushort)(PC + 0x200), new x8XY5(Opcode, this));
                                break;

                            case 0x0006: // 0x8XY6: Shifts VX right by one. VF is set to the value of the least significant bit of VX before the shift
                                ret.Add((ushort)(PC + 0x200), new x8XY6(Opcode, this));
                                break;

                            case 0x0007: // 0x8XY7: Sets VX to VY minus VX. VF is set to 0 when there's a borrow, and 1 when there isn't
                                ret.Add((ushort)(PC + 0x200), new x8XY7(Opcode, this));
                                break;

                            case 0x000E: // 0x8XYE: Shifts VX left by one. VF is set to the value of the most significant bit of VX before the shift
                                ret.Add((ushort)(PC + 0x200), new x8XYE(Opcode, this));
                                break;

                            default:
                                LogError("8000");
                                break;
                        }
                        break;

                    case 0x9000: // 0x9XY0: Skips the next instruction if VX doesn't equal VY
                        ret.Add((ushort)(PC + 0x200), new x9XY0(Opcode, this));
                        break;

                    case 0xA000: // ANNN: Sets I to the address NNN
                        ret.Add((ushort)(PC + 0x200), new xANNN(Opcode, this));
                        break;

                    case 0xB000: // BNNN: Jumps to the address NNN plus V0
                        ret.Add((ushort)(PC + 0x200), new xBNNN(Opcode, this));
                        break;

                    case 0xC000: // CXNN: Sets VX to a random number and NN
                        ret.Add((ushort)(PC + 0x200), new xCXNN(Opcode, this));
                        break;

                    case 0xD000: // DXYN: Draws a sprite at coordinate (VX, VY) that has a width of 8 pixels and a height of N pixels. 
                                 // Each row of 8 pixels is read as bit-coded starting from memory location I; 
                                 // I value doesn't change after the execution of this instruction. 
                                 // VF is set to 1 if any screen pixels are flipped from set to unset when the sprite is drawn, 
                                 // and to 0 if that doesn't happen
                        ret.Add((ushort)(PC + 0x200), new xDXYN(Opcode, this));
                        break;

                    case 0xE000:
                        switch (Opcode & 0x00FF)
                        {
                            case 0x009E: // EX9E: Skips the next instruction if the key stored in VX is pressed
                                ret.Add((ushort)(PC + 0x200), new xEX9E(Opcode, this));
                                break;

                            case 0x00A1: // EXA1: Skips the next instruction if the key stored in VX isn't pressed
                                ret.Add((ushort)(PC + 0x200), new xEXA1(Opcode, this));
                                break;

                            default:
                                LogError("E000");
                                break;
                        }
                        break;

                    case 0xF000:
                        switch (Opcode & 0x00FF)
                        {
                            case 0x0007: // FX07: Sets VX to the value of the delay timer
                                ret.Add((ushort)(PC + 0x200), new xFX07(Opcode, this));
                                break;

                            case 0x000A: // FX0A: A key press is awaited, and then stored in VX		
                                ret.Add((ushort)(PC + 0x200), new xFX0A(Opcode, this));
                                break;

                            case 0x0015: // FX15: Sets the delay timer to VX
                                ret.Add((ushort)(PC + 0x200), new xFX15(Opcode, this));
                                break;

                            case 0x0018: // FX18: Sets the sound timer to VX
                                ret.Add((ushort)(PC + 0x200), new xFX18(Opcode, this));
                                break;

                            case 0x001E: // FX1E: Adds VX to I
                                ret.Add((ushort)(PC + 0x200), new xFX1E(Opcode, this));
                                break;

                            case 0x0029: // FX29: Sets I to the location of the sprite for the character in VX. Characters 0-F (in hexadecimal) are represented by a 4x5 font
                                ret.Add((ushort)(PC + 0x200), new xFX29(Opcode, this));
                                break;

                            case 0x0033: // FX33: Stores the Binary-coded decimal representation of VX at the addresses I, I plus 1, and I plus 2
                                ret.Add((ushort)(PC + 0x200), new xFX33(Opcode, this));
                                break;

                            case 0x0055: // FX55: Stores V0 to VX in memory starting at address I					
                                ret.Add((ushort)(PC + 0x200), new xFX55(Opcode, this));
                                break;

                            case 0x0065: // FX65: Fills V0 to VX with values from memory starting at address I					
                                ret.Add((ushort)(PC + 0x200), new xFX65(Opcode, this));
                                break;

                            default:
                                LogError("F000");
                                break;
                        }
                        break;

                    default:
                        LogError("???");
                        break;
                }
            }

            return ret;
        }
    }
}