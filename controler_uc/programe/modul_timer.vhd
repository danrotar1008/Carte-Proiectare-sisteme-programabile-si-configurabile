----------------------------------------------------------------------------------
-- Company: 
-- Engineer: 
-- 
-- Create Date:    10:18:57 02/14/2015 
-- Design Name: 
-- Module Name:    timer1_top - Behavioral 
-- Project Name: 
-- Target Devices: 
-- Tool versions: 
-- Description: 
--
-- Dependencies: 
--
-- Revision: 
-- Revision 0.01 - File Created
-- Additional Comments: 
--
----------------------------------------------------------------------------------
library IEEE;
use IEEE.STD_LOGIC_1164.ALL;
use ieee.std_logic_unsigned.all;
--use ieee.numeric_std.all;

-- Uncomment the following library declaration if using
-- arithmetic functions with Signed or Unsigned values
--use IEEE.NUMERIC_STD.ALL;

-- Uncomment the following library declaration if instantiating
-- any Xilinx primitives in this code.
--library UNISIM;
--use UNISIM.VComponents.all;

entity timer is
	generic(MSB_prescaler : integer := 27); -- 2**28 = 268,435,456
	-- 100 MHz / 2**28 = aproximativ 0,4 Hz
    Port ( clk : in  STD_LOGIC;
           paralel : in  STD_LOGIC_VECTOR (7 downto 0); --valoarea care se inscrie in contor
			  contor : out STD_LOGIC_VECTOR (7 downto 0); --valoarea din contor
           tick : out  STD_LOGIC); -- impuls trimis cind contorul ajunge la valoarea maxima
end timer;

architecture Behavioral of timer is

signal prescaler:std_logic_vector(MSB_prescaler downto 0) := (others => '0');
signal tick_prescaler:std_logic;
signal temp_contor: std_logic_vector(7 downto 0);
signal tick_contor: std_logic;

begin

contoare_timer: process (clk, tick_contor, paralel)
begin
	if(clk'event and clk='1') then
		if tick_contor = '1' then
			prescaler <= (others => '0');
			temp_contor <= paralel;
		else
			prescaler <= prescaler + 1;
			if tick_prescaler = '1' then
				temp_contor <= temp_contor-1;
			else
				temp_contor <= temp_contor;
			end if;
		end if;
	end if;
end process;

tick_prescaler <= '1' when prescaler = (2**(MSB_prescaler+1) -1) else '0';
tick_contor <= '1' when temp_contor = "00000000" else '0';

contor <= temp_contor;
tick <= tick_contor;

end Behavioral;

