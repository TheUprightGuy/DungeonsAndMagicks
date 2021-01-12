void ClosestPointToLine_float(float3 linePnt, float3 lineDir, float3 pnt,out float3 pntOnLine)
{
    normalize(lineDir);//this needs to be a unit vector
    float3 v = pnt - linePnt;
    float d = dot(v, lineDir);
    pntOnLine = (linePnt + lineDir * d);
}