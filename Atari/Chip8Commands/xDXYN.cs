namespace Emulator.Chip8Commands
{
    /// <summary>
    /// Draws a sprite at coordinate (VX, VY) that has a width of 8 pixels and a height of N pixels. 
    /// Each row of 8 pixels is read as bit-coded starting from memory location I; 
    /// I value doesn't change after the execution of this instruction. 
    /// VF is set to 1 if any screen pixels are flipped from set to unset when the sprite is drawn, 
    /// and to 0 if that doesn't happen
    /// </summary>
    class xDXYN : BaseCommand
    {
        int X;
        int Y;
        ushort N;

        public xDXYN(ushort opcode, Chip8 chip) : base(chip) 
        {
            X = (opcode & 0x0F00) >> 8;
            Y = (opcode & 0x00F0) >> 4;
            N = (ushort)(opcode & 0x000F);
        }

        public override void Execute()
        {
            ushort x = V[X];
            ushort y = V[Y];
            ushort height = N;
            ushort pixel;

            V[0xF] = 0;
            for (int yline = 0; yline < height; yline++)
            {
                pixel = Chip.Memory[I + yline];
                for (int xline = 0; xline < 8; xline++)
                {
                    if ((pixel & (0x80 >> xline)) != 0)
                    {
                        int posVideo = (x + xline + ((y + yline) * 64));

                        if (Chip.GFX[posVideo] == 1)
                        {
                            V[0xF] = 1;
                        }

                        Chip.GFX[posVideo] ^= 1;
                    }
                }
            }

            Chip.DrawFlag = true;
            PC += 2;
        }
    }
}
