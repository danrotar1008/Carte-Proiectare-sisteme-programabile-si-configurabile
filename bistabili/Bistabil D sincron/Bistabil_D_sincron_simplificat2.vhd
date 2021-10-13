----------------------------------------------------------------------------------
-- Company: 
-- Engineer: 
-- 
-- Create Date: 13.10.2021 03:27:40
-- Design Name: 
-- Module Name: Bistabil_D_sincron - Behavioral
-- Project Name: 
-- Target Devices: 
-- Tool Versions: 
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

-- Uncomment the following library declaration if using
-- arithmetic functions with Signed or Unsigned values
--use IEEE.NUMERIC_STD.ALL;

-- Uncomment the following library declaration if instantiating
-- any Xilinx leaf cells in this code.
--library UNISIM;
--use UNISIM.VComponents.all;

entity Bistabil_D_sincron is
    Port ( 		
        clk, reset: in std_logic;
		en: in std_logic;
		d: in std_logic;
		q : out std_logic
);
end Bistabil_D_sincron;

architecture Behavioral of Bistabil_D_sincron is

begin

	process (clk, reset)
	begin
		if (reset = '1') then
			q <= '0';
		elsif (clk'event and clk = '0') then
			if (en = '1') then
				q <= d;
			end if;
		end if;
	end process;
    
end Behavioral;
