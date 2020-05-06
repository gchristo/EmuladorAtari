﻿namespace Emulator.Chip8Commands
{
    /// <summary>
    /// Skips the next instruction if VX doesn't equal NN
    /// </summary>
    public class x4XNN : BaseCommand
    {
        int X;
        int NN;

        public x4XNN(ushort opcode, Chip8 chip) : base(chip)
        {
            X = (opcode & 0x0F00) >> 8;
            NN = (opcode & 0x00FF);
        }

        public override void Execute()
        {
            if (V[X] != NN)
            {
                PC += 4;
            }
            else
            {
                PC += 2;
            }
        }
    }
}
