Shader "Mask/SplitScreen" {
	//Simple depthmask shader 
	SubShader {
	   
		ColorMask 0
		Zwrite On

		Pass { }
		// Tags {Queue = Background}
	   // Pass {ColorMask 0}
	}
}