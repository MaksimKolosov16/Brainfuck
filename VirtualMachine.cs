using System;
using System.Collections.Generic;

namespace func.brainfuck;

public class VirtualMachine : IVirtualMachine
{
	private readonly Dictionary<char, Action<IVirtualMachine>> _instructions = new();

	public string Instructions { get; }
	public int InstructionPointer { get; set; }
	public byte[] Memory { get; }
	public int MemoryPointer { get; set; }

	public VirtualMachine(string program, int memorySize)
	{
		Instructions = program;
		Memory = new byte[memorySize];
	}

	public void RegisterCommand(char symbol, Action<IVirtualMachine> execute)
	{
		_instructions.Add(symbol, execute);
	}

	public void Run()
	{
		for (; InstructionPointer < Instructions.Length; InstructionPointer++)
		{
			if (_instructions.TryGetValue(Instructions[InstructionPointer], out var instruction))
				instruction(this);
		}
	}
}