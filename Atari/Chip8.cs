using Emulator.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Emulator
{
    public abstract class Chip8
    {
        public delegate void DrawEventHandler(object sender, byte[] buffer, Size resolution);
        public event DrawEventHandler Draw;

        public delegate void PlaySoundEventHandler(object sender);
        public event PlaySoundEventHandler PlaySound;

        public byte[] Key = new byte[0x10]; //Input (0x0~0xF)

        public byte[] Memory = new byte[0x1000]; //Ram

        public ushort Timer_Sound; //when 0, beep

        public byte[] GFX = new byte[64 * 32]; //Graphic Buffer 
        public bool DrawFlag;

        public ushort Opcode; //Current opcode
        public byte[] V = new byte[0x10]; //Register
        public ushort I; //Current index
        public ushort PC = 0x200; //Program Counter

        public ushort Timer_Delay; //gameLoop Timer 

        public ushort[] Stack = new ushort[0x10]; //Execution Stack
        public byte SP; //Stack Pointer

        public Size Resolution = new Size(64, 32);

        byte[] chip8_fontset = new byte[80]
        {
            0xF0, 0x90, 0x90, 0x90, 0xF0, // 0
            0x20, 0x60, 0x20, 0x20, 0x70, // 1
            0xF0, 0x10, 0xF0, 0x80, 0xF0, // 2
            0xF0, 0x10, 0xF0, 0x10, 0xF0, // 3
            0x90, 0x90, 0xF0, 0x10, 0x10, // 4
            0xF0, 0x80, 0xF0, 0x10, 0xF0, // 5
            0xF0, 0x80, 0xF0, 0x90, 0xF0, // 6
            0xF0, 0x10, 0x20, 0x40, 0x40, // 7
            0xF0, 0x90, 0xF0, 0x90, 0xF0, // 8
            0xF0, 0x90, 0xF0, 0x10, 0xF0, // 9
            0xF0, 0x90, 0xF0, 0x90, 0x90, // A
            0xE0, 0x90, 0xE0, 0x90, 0xE0, // B
            0xF0, 0x80, 0x80, 0x80, 0xF0, // C
            0xE0, 0x90, 0x90, 0x90, 0xE0, // D
            0xF0, 0x80, 0xF0, 0x80, 0xF0, // E
            0xF0, 0x80, 0xF0, 0x80, 0x80  // F
        };

        Dictionary<Keys, int> Inputs = new Dictionary<Keys, int>()
        {
            { Keys.D1,  0x1 }, { Keys.D2, 0x2 }, { Keys.D3, 0x3 }, { Keys.D4, 0xC },
            { Keys.Q, 0x4 }, { Keys.W, 0x5 }, { Keys.E, 0x6 }, { Keys.R, 0xD },
            { Keys.A, 0x7 }, { Keys.S, 0x8 }, { Keys.D, 0x9 }, { Keys.F, 0xE },
            { Keys.Z, 0xA }, { Keys.X, 0x0 }, { Keys.C, 0xB }, { Keys.V, 0xF }
        };

        public abstract void ProcessorStep();
        public abstract void LoadGame(byte[] gameData);

        public State State
        {
            get => new State()
            {
                DrawFlag = DrawFlag,
                GFX = GFX,
                I = I,
                Memory = Memory,
                Opcode = Opcode,
                PC = PC,
                SP = SP,
                Stack = Stack,
                Timer_Delay = Timer_Delay,
                Timer_Sound = Timer_Sound,
                V = V
            };

            set
            {
                if (value != null)
                {
                    DrawFlag = value.DrawFlag;
                    GFX = value.GFX;
                    Memory = value.Memory;
                    Timer_Sound = value.Timer_Sound;
                    Opcode = value.Opcode;
                    V = value.V;
                    I = value.I;
                    PC = value.PC;
                    Timer_Delay = value.Timer_Delay;
                    Stack = value.Stack;
                    SP = value.SP;
                }
            }
        }

        public void Run()
        {
            ProcessorCycle.Cycle(500, () =>
            {
                //Benchmark.Log(() =>
                //{
                ProcessorStep();
                //});

                if (Timer_Delay > 0)
                    --Timer_Delay;

                if (Timer_Sound > 0)
                {
                    if (Timer_Sound == 1)
                        PlaySound?.Invoke(this);

                    --Timer_Sound;
                }

                if (DrawFlag)
                {
                    Draw?.Invoke(this, GFX, Resolution);
                    DrawFlag = false;
                }
            });
        }

        protected ushort GetOpcode()
        {
            return Opcode = (ushort)(Memory[PC] << 8 | Memory[PC + 1]);
        }

        public void Reset()
        {
            PC = 0x200; // Program counter starts at 0x200
            Opcode = 0; // Reset current opcode	
            I = 0;      // Reset index register

            ClearStack();
            ClearRegisters();

            ClearDisplay();
            MemoryReset();
        }

        public void ClearDisplay()
        {
            for (int i = 0; i < GFX.Length; ++i)
            {
                GFX[i] = 0;
            }
        }

        public void ClearDisplay(bool draw)
        {
            ClearDisplay();
            DrawFlag = draw;
        }

        protected void ClearStack()
        {
            for (var i = 0; i < Stack.Length; ++i)
            {
                Stack[i] = 0;
            }

            SP = 0;     // Reset stack pointer
        }

        protected void ClearRegisters()
        {
            for (var i = 0; i < V.Length; ++i)
            {
                V[i] = 0;
            }
        }

        public void KeyDown(Keys key) => SetKeyValue(key, 1);
        public void KeyUp(Keys key) => SetKeyValue(key, 0);

        private void SetKeyValue(Keys key, byte value)
        {
            if (Inputs.ContainsKey(key))
            {
                Key[Inputs[key]] = value;
            }
        }

        public void MemoryReset()
        {
            ClearMemory();
            LoadFontSet();
        }

        protected void LoadFontSet()
        {
            for (int i = 0; i < 80; ++i)
            {
                Memory[i] = chip8_fontset[i];
            }
        }

        private void ClearMemory()
        {
            for (var i = 0; i < Memory.Length; ++i)
            {
                Memory[i] = 0;
            }
        }

        protected void LoadGameToMemory(byte[] gameData)
        {
            for (int i = 0; i < gameData.Length; ++i)
            {
                Memory[i + 0x200] = gameData[i];
            }
        }

        public void LogError(string segment)
        {
            Console.WriteLine($"Unknown opcode [0x{segment}]: 0x{GetOpcode()} / PC: {PC}");
        }
    }
}
