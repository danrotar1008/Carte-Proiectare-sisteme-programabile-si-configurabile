----------------------------------------------------------------------------------
-- Company: 
-- Engineer: 
-- 
-- Create Date: 12.10.2021 11:18:12
-- Design Name: 
-- Module Name: decodor - Behavioral
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

entity decodor is
    Port ( hex : in STD_LOGIC_VECTOR (3 downto 0); -- intrare binara
           dp : in STD_LOGIC; -- punctul zecimal
           sseg : out STD_LOGIC_VECTOR (7 downto 0));-- iesire 7 segmente
end decodor;

architecture Behavioral of decodor is

begin

	with hex select
		sseg(6 downto 0) <=
			"0000001" when "0000",
			"1001111" when "0001",
			"0010010" when "0010",
			"0000110" when "0011",
			"1001100" when "0100",
			"0100100" when "0101",
			"0100000" when "0110",
			"0001111" when "0111",
			"0000000" when "1000",
			"0000100" when "1001",
			"0001000" when "1010", --a
			"1100000" when "1011", --b
			"0110001" when "1100", --c
			"1000010" when "1101", --d
			"0110000" when "1110", --e
			"0111000" when others; --f
	sseg(7) <= dp;

end Behavioral;
