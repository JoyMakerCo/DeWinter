using System;
using System.Collections;
using System.Collections.Generic;

namespace Ambition
{
	public class RemarkVO
	{
	    public int Profile; // int acting as an array of bits.
	    public string Topic;

	    public bool IsAmbush
	    {
	    	get { return Profile == 0; }
	    }
	}
}