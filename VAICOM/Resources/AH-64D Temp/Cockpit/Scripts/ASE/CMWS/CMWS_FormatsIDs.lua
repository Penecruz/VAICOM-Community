
local count = -1
local function counter()
	count = count + 1
	return count
end

CMWS_DISPL_FMT_LEV1 =
{
	BLANK	= counter(),
	MAIN	= counter(),
	TEST	= counter(),
}

CMWS_DISPL_FMT_LEV2 =
{
	NONE	= 0,
}

CMWS_DISPL_FMT_LEV3 =
{
	NONE	= 0,
}

CMWS_DISPL_FMT_LEV4 =
{
	NONE	= 0,
}