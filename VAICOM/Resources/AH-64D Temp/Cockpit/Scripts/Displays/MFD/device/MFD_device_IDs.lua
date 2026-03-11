MFD_SELF_IDS =
{
	PLT_LMFD	= 0,
	PLT_RMFD	= 1,
	CPG_LMFD	= 2,
	CPG_RMFD	= 3,
	TEDAC		= 4,
}

function getMFD_suffix(mfd_id)
	if mfd_id == MFD_SELF_IDS.PLT_LMFD then
		return "_PLT_LMFD";
	elseif mfd_id == MFD_SELF_IDS.PLT_RMFD then
		return "_PLT_RMFD";
	elseif mfd_id == MFD_SELF_IDS.CPG_LMFD then
		return "_CPG_LMFD";
	elseif mfd_id == MFD_SELF_IDS.CPG_RMFD then
		return "_CPG_RMFD";
	elseif mfd_id == MFD_SELF_IDS.TEDAC then
		return "_TEDAC";
	end
	return "";
end