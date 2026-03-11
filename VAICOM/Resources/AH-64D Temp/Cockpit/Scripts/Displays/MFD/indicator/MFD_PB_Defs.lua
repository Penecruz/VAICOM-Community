
local count = 0
local function counter()
	count = count + 1
	return count
end

pb = {}

pb.T1	= counter() -- =1
pb.T2	= counter()
pb.T3	= counter()
pb.T4	= counter()
pb.T5	= counter()
pb.T6	= counter()
pb.R1	= counter()
pb.R2	= counter()
pb.R3	= counter()
pb.R4	= counter()
pb.R5	= counter()
pb.R6	= counter()
pb.B6	= counter()
pb.B5	= counter()
pb.B4	= counter()
pb.B3	= counter()
pb.B2	= counter()
pb.B1	= counter()
pb.L6	= counter()
pb.L5	= counter()
pb.L4	= counter()
pb.L3	= counter()
pb.L2	= counter()
pb.L1	= counter()
pb.MAX	= counter()

local LXLeftPos		= -493 -- +
local RXLeftPos		=  484 -- +
local TYLeftPos		=  485 -- +
local DYLeftPos		= -495 -- +
--
local TDXLeftPos	= -335	--TD - top and down menu
local TDXRightPos	=  320
-- LRYLeftPos
local LRYTopPos		=  330	--LR - left and right menu
local LRYBottomPos	= -280


PB_dist_x		= (TDXRightPos - TDXLeftPos) / 5
PB_dist_y		= (LRYTopPos - LRYBottomPos) / 5

--
PB_Pos = {}

for pb_num = pb.T1, pb.L1 do
	if	pb_num >= pb.L6 then
		PB_Pos[pb_num]			= {LXLeftPos, LRYTopPos - PB_dist_y * (pb.L1 - pb_num )}
	elseif	pb_num >= pb.B6 then
		PB_Pos[pb_num]			= {TDXLeftPos + PB_dist_x * ( pb.B1 - pb_num ), DYLeftPos}
	elseif	pb_num >= pb.R1 then
		PB_Pos[pb_num]			= {RXLeftPos, LRYTopPos - PB_dist_y * (pb_num - pb.R1)}
	elseif	pb_num >= pb.T1 then
		PB_Pos[pb_num]			= {TDXLeftPos + PB_dist_x * (pb_num - pb.T1), TYLeftPos}
	end
end