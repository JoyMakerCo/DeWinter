using System;
using System.Collections;
using System.Collections.Generic;

namespace Ambition
{
	public class RemarkVO
	{
		public int NumTargets=0;
	    public string Interest;
	    public bool IsAmbush
	    {
	    	get { return NumTargets == 0; }
	    }

		public RemarkVO() {}
	    public RemarkVO(int numTargets, string interest)
	    {
			NumTargets = numTargets;
			Interest = interest;
	    }
	}
}
