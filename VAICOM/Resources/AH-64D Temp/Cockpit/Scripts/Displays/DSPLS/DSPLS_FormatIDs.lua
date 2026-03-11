local count = -1
local function counter()
	count = count + 1
	return count
end

local function reset_counter()
	count = -1
end


reset_counter()
DSPLS_DISPL_FMT_LEV1 =
{
	BLANK		= counter(),
	EMPTY		= counter(),
	GRAYSCALE	= counter(),
	FLIGHT		= counter(),
	WEAPON		= counter(),
}

reset_counter()
DSPLS_DISPL_FMT_LEV2 =
{
	NONE		= counter(),
	HOVER		= counter(),
	BOB_UP		= counter(),
	TRANSITION	= counter(),
	CRUISE		= counter(),
	WEAPON		= counter(),
}

DSPLS_DISPL_FMT_LEV3 =
{
	NONE	= 0,
}

DSPLS_DISPL_FMT_LEV4 =
{
	NONE	= 0,
}