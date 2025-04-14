dofile('./Scripts/UI/initGUI.lua')

local base = _G

module('gameMessages')

local require = base.require
local pairs = base.pairs
local ipairs = base.ipairs
local print = base.print
local table = base.table
local string = base.string
local tonumber = base.tonumber
local tostring = base.tostring
local assert = base.assert
local type = base.type
local math = base.math
local os = base.os
local HMD = base.HMD

local Gui               = require('dxgui')
local DialogLoader      = require('DialogLoader')
local gettext           = require('i_18n')
local DCS               = require('DCS')

base.setmetatable(base.dxgui, {__index = base.dxguiWin})

local listMessages = {}
local timeShow = 30000
local timeAnim = 5000

local timeAnimPause = 0
local timeStartAnimPause = 0

local ListCreatedWidgets = {}

local autoScrollTextTrig
local autoScrollTextRadio

local autoScrollTextTrigSkin
local autoScrollTextRadioSkin
local autoScrollTextTrigHmdSkin
local autoScrollTextRadioHmdSkin

local function _(text) 
    if text == nil then
        return ""
    end
    return gettext.translate(text) 
end

local function getLentaTriggerBounds(a_h)
	local offsetTop		= 34
	local offsetLeft	= 20
	local width
	
	if HMD.isActive() then
		width = main_w / 2
	else
		width = 600
	end
	
	local x = main_w - width - offsetLeft
	local y = offsetTop + a_h
	local height = main_h - offsetTop - a_h
	
	return x, y, width, height
end

local function getFontScale()
	local fontScale =  DCS.getUserOptions().graphics.messagesFontScale	or 1
	
	if HMD.isActive() then
		fontScale = fontScale * 1.5
	end
	
	return fontScale
end

local function setFontScale(widget, autoScrollTextSkin, fontScale)
	local text = autoScrollTextSkin.skinData.skins.text.skinData.states.released[1].text
	local fontSize = text.fontSize
	
	text.fontSize = text.fontSize * fontScale
	widget:setSkin(autoScrollTextSkin)
	
	text.fontSize = fontSize
end

local function selectHmdSkin()
    if HMD.isActive() then
		autoScrollTextTrig:setSkin(autoScrollTextTrigHmdSkin)
		autoScrollTextRadio:setSkin(autoScrollTextRadioHmdSkin)
      
        window.sRadioAuto:setVisible(false)
        sRadioAuto = window.sRadioAuto_High
        window.sMain:setVisible(false)
        sMain = window.sMain_High
    else
		local fontScale = DCS.getUserOptions().graphics.messagesFontScale	or 1		
		
		setFontScale(autoScrollTextTrig, autoScrollTextTrigSkin, fontScale)
		setFontScale(autoScrollTextRadio, autoScrollTextRadioSkin, fontScale)
        
        window.sMain_High:setVisible(false)
        sMain = window.sMain
        window.sRadioAuto_High:setVisible(false)    
        sRadioAuto = window.sRadioAuto
    end
end

function create()
    local localization = {
	}

	window = DialogLoader.spawnDialogFromFile('Scripts/UI/gameMessages.dlg', localization)
    main_w, main_h = Gui.GetWindowSize()
    window:setBounds(0, 0, main_w, main_h)  
    
    sPause = window.sPause
    
    sPause:setPosition(main_w-76,9)
	
	autoScrollTextTrigSkin		= window.autoScrollTextTrig:getSkin()
	autoScrollTextRadioSkin		= window.autoScrollTextRadio:getSkin()
	autoScrollTextTrigHmdSkin	= window.autoScrollTextTrig_High:getSkin()
	autoScrollTextRadioHmdSkin	= window.autoScrollTextRadio_High:getSkin()
    
	autoScrollTextTrig = window.autoScrollTextTrig
	autoScrollTextRadio = window.autoScrollTextRadio
    
	selectHmdSkin()

    autoScrollTextTrig:setBounds(getLentaTriggerBounds(0))
	autoScrollTextRadio:setBounds(0, 34, main_w - 650, main_h - 54)
    
    sPause:setVisible(false)
    onRadioCommand("")
end

function setOffsetLentaTrigger(a_h)	
	if autoScrollTextTrig then
		autoScrollTextTrig:setBounds(getLentaTriggerBounds(a_h))
	end
end

local function makeExist()
	if not window then 
		create()
	end
end

function show()
    makeExist()
    
    selectHmdSkin()
	
    window:setVisible(true)
end

function clear()
	if window then
		autoScrollTextTrig:clear()
		autoScrollTextRadio:clear()
		sRadioAuto:setText("")
		sRadioAuto:setVisible(false)
		sMain:setVisible(false)
		
		autoScrollTextTrig:setBounds(getLentaTriggerBounds(0)) --возвращаем назад для новой миссии
	end
end

function hide()
    if window then
        window:setVisible(false)
    end
end


function addTriggerMessage(a_text, a_duration, a_clearView)
    makeExist()
	if a_clearView == true or a_clearView == 1 then
		autoScrollTextTrig:clear()
    end
    --base.print("----gameMessages---addTriggerMessage()",a_text, a_duration, pLentaTrigger, a_clearView )
   
	if autoScrollTextTrig then
		autoScrollTextTrig:addText(a_text, a_duration)
	end
end 

function addRadioMessage(a_text, a_duration)
	makeExist()
	if autoScrollTextRadio then
		autoScrollTextRadio:addText(a_text, a_duration)
	end
end    

function onRadioCommand(a_command_message)
    sRadioAuto:setText(a_command_message)
    if a_command_message == "" then
        sRadioAuto:setVisible(false)
        sMain:setVisible(false)
    else   
        sRadioAuto:setVisible(true)
        sMain:setVisible(true)
    end
end
    
function addMessage(a_text, a_duration)
	makeExist()
    --base.print("----gameMessages---addMessage()")
    autoScrollTextTrig:addText(a_text, a_duration / 1000)
end

function updateAnimations()    
    if sPause and sPause:getVisible() then
        local timeCur = DCS.getRealTime() * 1000 - timeStartAnimPause        
        if (timeCur > 2000) then
            timeStartAnimPause = DCS.getRealTime() * 1000
            timeCur = 0
        end
        sPause:setOpacity((1500 - timeCur) / 1500)
    end
end

function showPause()
    if sPause then
        if DCS.isMultiplayer() == true then
            sPause:setVisible(true)
            timeStartAnimPause = DCS.getRealTime() * 1000
            sPause:setOpacity(1)
        else
            sPause:setVisible(false)
        end
    end    
end

function hidePause()
    if sPause then
        sPause:setVisible(false)
    end    
end

function kill()
    for k,v in base.pairs(ListCreatedWidgets) do
        v:destroy()
    end
    
	if window_ then
	   window_:setVisible(false)
	   window_:kill()
	   window_ = nil
	end
end
