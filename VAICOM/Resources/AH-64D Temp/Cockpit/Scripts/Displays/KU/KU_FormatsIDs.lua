
local count = -1
local function counter()
	count = count + 1
	return count
end

KU_DISPL_FMT_LEV1 =
{
	BLANK	= counter(),
	TEST	= counter(),
	MAIN	= counter(),
}

KU_DISPL_FMT_LEV2 =
{
	NONE	= 0,
}

KU_DISPL_FMT_LEV3 =
{
	NONE	= 0,
}

KU_DISPL_FMT_LEV4 =
{
	NONE	= 0,
}