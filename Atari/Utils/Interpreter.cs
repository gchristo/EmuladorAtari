namespace Emulator.Utils
{
    public class Interpreter : Chip8
    {
        public override void ProcessorStep()
        {
            GetOpcode();
            DecodeUpcode();
        }

        public override void LoadGame(byte[] gameData)
        {
            LoadGameToMemory(gameData);
        }

        private void DecodeUpcode()
        {
            switch (Opcode & 0xF000)
            {
                case 0x0000:
                    switch (Opcode & 0x000F)
                    {
                        case 0x0000: // 0x00E0: Clears the screen
                            ClearDisplay(true);
                            PC += 2;
                            break;

                        case 0x000E: // 0x00EE: Returns from subroutine
                            --SP;           // 16 levels of stack, decrease stack pointer to prevent overwrite
                            PC = Stack[SP]; // Put the stored return address from the stack back into the program counter					
                            PC += 2;        // Don't forget to increase the program counter!
                            break;

                        default:
                            LogError("0000");
                            break;
                    }
                    break;

                case 0x1000: // 0x1NNN: Jumps to address NNN
                    PC = (ushort)(Opcode & 0x0FFF);
                    break;

                case 0x2000: // 0x2NNN: Calls subroutine at NNN.
                    Stack[SP] = PC;         // Store current address in stack
                    ++SP;                   // Increment stack pointer
                    PC = (ushort)(Opcode & 0x0FFF);   // Set the program counter to the address at NNN
                    break;

                case 0x3000: // 0x3XNN: Skips the next instruction if VX equals NN
                    if (V[(Opcode & 0x0F00) >> 8] == (Opcode & 0x00FF))
                        PC += 4;
                    else
                        PC += 2;
                    break;

                case 0x4000: // 0x4XNN: Skips the next instruction if VX doesn't equal NN
                    if (V[(Opcode & 0x0F00) >> 8] != (Opcode & 0x00FF))
                        PC += 4;
                    else
                        PC += 2;
                    break;

                case 0x5000: // 0x5XY0: Skips the next instruction if VX equals VY.
                    if (V[(Opcode & 0x0F00) >> 8] == V[(Opcode & 0x00F0) >> 4])
                        PC += 4;
                    else
                        PC += 2;
                    break;

                case 0x6000: // 0x6XNN: Sets VX to NN.
                    V[(Opcode & 0x0F00) >> 8] = (byte)(Opcode & 0x00FF);
                    PC += 2;
                    break;

                case 0x7000: // 0x7XNN: Adds NN to VX.
                    V[(Opcode & 0x0F00) >> 8] += (byte)(Opcode & 0x00FF);
                    PC += 2;
                    break;

                case 0x8000:
                    switch (Opcode & 0x000F)
                    {
                        case 0x0000: // 0x8XY0: Sets VX to the value of VY
                            V[(Opcode & 0x0F00) >> 8] = V[(Opcode & 0x00F0) >> 4];
                            PC += 2;
                            break;

                        case 0x0001: // 0x8XY1: Sets VX to "VX OR VY"
                            V[(Opcode & 0x0F00) >> 8] |= V[(Opcode & 0x00F0) >> 4];
                            PC += 2;
                            break;

                        case 0x0002: // 0x8XY2: Sets VX to "VX AND VY"
                            V[(Opcode & 0x0F00) >> 8] &= V[(Opcode & 0x00F0) >> 4];
                            PC += 2;
                            break;

                        case 0x0003: // 0x8XY3: Sets VX to "VX XOR VY"
                            V[(Opcode & 0x0F00) >> 8] ^= V[(Opcode & 0x00F0) >> 4];
                            PC += 2;
                            break;

                        case 0x0004: // 0x8XY4: Adds VY to VX. VF is set to 1 when there's a carry, and to 0 when there isn't					
                            if (V[(Opcode & 0x00F0) >> 4] > (0xFF - V[(Opcode & 0x0F00) >> 8]))
                                V[0xF] = 1; //carry
                            else
                                V[0xF] = 0;

                            V[(Opcode & 0x0F00) >> 8] += V[(Opcode & 0x00F0) >> 4];
                            PC += 2;
                            break;

                        case 0x0005: // 0x8XY5: VY is subtracted from VX. VF is set to 0 when there's a borrow, and 1 when there isn't
                            if (V[(Opcode & 0x00F0) >> 4] > V[(Opcode & 0x0F00) >> 8])
                                V[0xF] = 0; // there is a borrow
                            else
                                V[0xF] = 1;
                            V[(Opcode & 0x0F00) >> 8] -= V[(Opcode & 0x00F0) >> 4];

                            PC += 2;
                            break;

                        case 0x0006: // 0x8XY6: Shifts VX right by one. VF is set to the value of the least significant bit of VX before the shift
                            V[0xF] = (byte)(V[(Opcode & 0x0F00) >> 8] & 0x1);
                            V[(Opcode & 0x0F00) >> 8] >>= 1;
                            PC += 2;
                            break;

                        case 0x0007: // 0x8XY7: Sets VX to VY minus VX. VF is set to 0 when there's a borrow, and 1 when there isn't
                            if (V[(Opcode & 0x0F00) >> 8] > V[(Opcode & 0x00F0) >> 4])  // VY-VX
                                V[0xF] = 0; // there is a borrow
                            else
                                V[0xF] = 1;
                            V[(Opcode & 0x0F00) >> 8] = (byte)(V[(Opcode & 0x00F0) >> 4] - V[(Opcode & 0x0F00) >> 8]);
                            PC += 2;
                            break;

                        case 0x000E: // 0x8XYE: Shifts VX left by one. VF is set to the value of the most significant bit of VX before the shift
                            V[0xF] = (byte)(V[(Opcode & 0x0F00) >> 8] >> 7);
                            V[(Opcode & 0x0F00) >> 8] <<= 1;
                            PC += 2;
                            break;

                        default:
                            LogError("8000");
                            break;
                    }
                    break;

                case 0x9000: // 0x9XY0: Skips the next instruction if VX doesn't equal VY
                    if (V[(Opcode & 0x0F00) >> 8] != V[(Opcode & 0x00F0) >> 4])
                        PC += 4;
                    else
                        PC += 2;
                    break;

                case 0xA000: // ANNN: Sets I to the address NNN
                    I = (ushort)(Opcode & 0x0FFF);
                    PC += 2;
                    break;

                case 0xB000: // BNNN: Jumps to the address NNN plus V0
                    PC = (ushort)((Opcode & 0x0FFF) + V[0]);
                    break;

                case 0xC000: // CXNN: Sets VX to a random number and NN
                    V[(Opcode & 0x0F00) >> 8] = (byte)((Extensions.GetRandon(0xFF)) & (Opcode & 0x00FF));
                    PC += 2;
                    break;

                case 0xD000: // DXYN: Draws a sprite at coordinate (VX, VY) that has a width of 8 pixels and a height of N pixels. 
                             // Each row of 8 pixels is read as bit-coded starting from memory location I; 
                             // I value doesn't change after the execution of this instruction. 
                             // VF is set to 1 if any screen pixels are flipped from set to unset when the sprite is drawn, 
                             // and to 0 if that doesn't happen
                    {
                        ushort x = V[(Opcode & 0x0F00) >> 8];
                        ushort y = V[(Opcode & 0x00F0) >> 4];
                        ushort height = (ushort)(Opcode & 0x000F);
                        ushort pixel;

                        V[0xF] = 0;
                        for (int yline = 0; yline < height; yline++)
                        {
                            pixel = Memory[I + yline];
                            for (int xline = 0; xline < 8; xline++)
                            {
                                if ((pixel & (0x80 >> xline)) != 0)
                                {
                                    if (GFX[(x + xline + ((y + yline) * 64))] == 1)
                                        V[0xF] = 1;

                                    GFX[x + xline + ((y + yline) * 64)] ^= 1;
                                }
                            }
                        }

                        DrawFlag = true;
                        PC += 2;
                    }
                    break;

                case 0xE000:
                    switch (Opcode & 0x00FF)
                    {
                        case 0x009E: // EX9E: Skips the next instruction if the key stored in VX is pressed
                            if (Key[V[(Opcode & 0x0F00) >> 8]] != 0)
                                PC += 4;
                            else
                                PC += 2;
                            break;

                        case 0x00A1: // EXA1: Skips the next instruction if the key stored in VX isn't pressed
                            if (Key[V[(Opcode & 0x0F00) >> 8]] == 0)
                                PC += 4;
                            else
                                PC += 2;
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
                            V[(Opcode & 0x0F00) >> 8] = (byte)Timer_Delay;
                            PC += 2;
                            break;

                        case 0x000A: // FX0A: A key press is awaited, and then stored in VX		
                            bool keyPress = false;

                            for (int i = 0; i < 16; ++i)
                            {
                                if (Key[i] != 0)
                                {
                                    V[(Opcode & 0x0F00) >> 8] = (byte)I;
                                    keyPress = true;
                                }
                            }

                            // If we didn't received a keypress, skip this cycle and try again.
                            if (!keyPress)
                                return;

                            PC += 2;
                            break;

                        case 0x0015: // FX15: Sets the delay timer to VX
                            Timer_Delay = V[(Opcode & 0x0F00) >> 8];
                            PC += 2;
                            break;

                        case 0x0018: // FX18: Sets the sound timer to VX
                            Timer_Sound = V[(Opcode & 0x0F00) >> 8];
                            PC += 2;
                            break;

                        case 0x001E: // FX1E: Adds VX to I
                            if (I + V[(Opcode & 0x0F00) >> 8] > 0xFFF)  // VF is set to 1 when range overflow (I+VX>0xFFF), and 0 when there isn't.
                                V[0xF] = 1;
                            else
                                V[0xF] = 0;
                            I += V[(Opcode & 0x0F00) >> 8];
                            PC += 2;
                            break;

                        case 0x0029: // FX29: Sets I to the location of the sprite for the character in VX. Characters 0-F (in hexadecimal) are represented by a 4x5 font
                            I = (ushort)(V[(Opcode & 0x0F00) >> 8] * 0x5);
                            PC += 2;
                            break;

                        case 0x0033: // FX33: Stores the Binary-coded decimal representation of VX at the addresses I, I plus 1, and I plus 2
                            Memory[I] = (byte)(V[(Opcode & 0x0F00) >> 8] / 100);
                            Memory[I + 1] = (byte)((V[(Opcode & 0x0F00) >> 8] / 10) % 10);
                            Memory[I + 2] = (byte)((V[(Opcode & 0x0F00) >> 8] % 100) % 10);
                            PC += 2;
                            break;

                        case 0x0055: // FX55: Stores V0 to VX in memory starting at address I					
                            for (int i = 0; i <= ((Opcode & 0x0F00) >> 8); ++i)
                                Memory[I + i] = V[i];

                            // On the original interpreter, when the operation is done, I = I + X + 1.
                            I += (ushort)(((Opcode & 0x0F00) >> 8) + 1);
                            PC += 2;
                            break;

                        case 0x0065: // FX65: Fills V0 to VX with values from memory starting at address I					
                            for (int i = 0; i <= ((Opcode & 0x0F00) >> 8); ++i)
                                V[i] = Memory[I + i];

                            // On the original interpreter, when the operation is done, I = I + X + 1.
                            I += (ushort)(((Opcode & 0x0F00) >> 8) + 1);
                            PC += 2;
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
    }
}
