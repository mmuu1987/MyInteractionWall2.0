#ifndef _MATH_INCLUDED_
#define _MATH_INCLUDED_

#define QUATERNION_IDENTITY float4(0, 0, 0, 1)


//#define RadianRatio 57.29578;
//????ת?Ƕ?
#define RadianRatio 57.29578
#define P 3.141592653

// Quaternion multiplication
// http://mathworld.wolfram.com/Quaternion.html
float4 qmul(float4 q1, float4 q2) {
	return float4(
		q2.xyz * q1.w + q1.xyz * q2.w + cross(q1.xyz, q2.xyz),
		q1.w * q2.w - dot(q1.xyz, q2.xyz)
	);
}

// Vector rotation with a quaternion
// http://mathworld.wolfram.com/Quaternion.html
float3 rotate_vector(float3 v, float4 r) {
	float4 r_c = r * float4(-1, -1, -1, 1);
	return qmul(r, qmul(float4(v, 0), r_c)).xyz;
}

float3 rotate_vector_at(float3 v, float3 center, float4 r) {
	float3 dir = v - center;
	return center + rotate_vector(dir, r);
}

// A given angle of rotation about a given axis
float4 rotate_angle_axis(float angle, float3 axis) {
	float sn = sin(angle * 0.5);
	float cs = cos(angle * 0.5);
	return float4(axis * sn, cs);
}


float4 q_conj(float4 q) {
	return float4(-q.x, -q.y, -q.z, q.w);
}

float4x4 look_at_matrix(float3 at, float3 eye, float3 up) {
	float3 zaxis = normalize(at - eye);
	float3 xaxis = normalize(cross(up, zaxis));
	float3 yaxis = cross(zaxis, xaxis);
	return float4x4(
		xaxis.x, yaxis.x, zaxis.x, 0,
		xaxis.y, yaxis.y, zaxis.y, 0,
		xaxis.z, yaxis.z, zaxis.z, 0,
		0, 0, 0, 1
	);
}

// http://stackoverflow.com/questions/349050/calculating-a-lookat-matrix
float4x4 look_at_matrix(float3 forward, float3 up) {
	float3 xaxis = cross(forward, up);
	float3 yaxis = up;
	float3 zaxis = forward;
	return float4x4(
		xaxis.x, yaxis.x, zaxis.x, 0,
		xaxis.y, yaxis.y, zaxis.y, 0,
		xaxis.z, yaxis.z, zaxis.z, 0,
		0, 0, 0, 1
	);
}

// http://www.euclideanspace.com/maths/geometry/rotations/conversions/matrixToQuaternion/
float4 matrix_to_quaternion(float4x4 m) {

	float tr = m[0][0] + m[1][1] + m[2][2];
	float4 q = float4(0, 0, 0, 0);

	if (tr > 0) {
		float s = sqrt(tr + 1.0) * 2; // S=4*qw 
		q.w = 0.25 * s;
		q.x = (m[2][1] - m[1][2]) / s;
		q.y = (m[0][2] - m[2][0]) / s;
		q.z = (m[1][0] - m[0][1]) / s;
	} else if ((m[0][0] > m[1][1]) && (m[0][0] > m[2][2])) {
		float s = sqrt(1.0 + m[0][0] - m[1][1] - m[2][2]) * 2; // S=4*qx 
		q.w = (m[2][1] - m[1][2]) / s;
		q.x = 0.25 * s;
		q.y = (m[0][1] + m[1][0]) / s;
		q.z = (m[0][2] + m[2][0]) / s;
	} else if (m[1][1] > m[2][2]) {
		float s = sqrt(1.0 + m[1][1] - m[0][0] - m[2][2]) * 2; // S=4*qy
		q.w = (m[0][2] - m[2][0]) / s;
		q.x = (m[0][1] + m[1][0]) / s;
		q.y = 0.25 * s;
		q.z = (m[1][2] + m[2][1]) / s;
	} else {
		float s = sqrt(1.0 + m[2][2] - m[0][0] - m[1][1]) * 2; // S=4*qz
		q.w = (m[1][0] - m[0][1]) / s;
		q.x = (m[0][2] + m[2][0]) / s;
		q.y = (m[1][2] + m[2][1]) / s;
		q.z = 0.25 * s;
	}

	return q;
}



 float EaseInQuad(float start, float end, float value)
    {
        end -= start;
        return end * value * value + start;
    }

   float EaseOutQuad(float start, float end, float value)
    {
        end -= start;
        return -end * value * (value - 2) + start;
    }

   


#endif // _MATH_INCLUDED_
