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
    Port ( clk : in STD_LOGIC;
           reset : in STD_LOGIC;
           en : in STD_LOGIC;
           d : in STD_LOGIC;
           q : out STD_LOGIC;
           nq : out STD_LOGIC);
end Bistabil_D_sincron;

architecture Behavioral of Bistabil_D_sincron is

signal r_reg, r_next: std_logic;

begin
-- D_sincron
	process (clk, reset)
	begin
		if (reset = '1') then
			r_reg <= '0';
		elsif (clk'event and clk='1') then
			r_reg <= r_next;
		end if ;
	end process;
-- next - state logic
	r_next  <= d when en = '1' else
	r_reg;
-- output logic
	q <= r_reg;
    nq <= not r_reg;
    
end Behavioral;
