----------------------------------------------------------------------------------
-- Company: 
-- Engineer: 
-- 
-- Create Date: 12.10.2021 13:01:25
-- Design Name: 
-- Module Name: decodor_TB - Behavioral
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
use IEEE.NUMERIC_STD.ALL;

-- Uncomment the following library declaration if instantiating
-- any Xilinx leaf cells in this code.
--library UNISIM;
--use UNISIM.VComponents.all;

entity decodor_TB is
--  Port ( );
end decodor_TB;

architecture Behavioral of decodor_TB is

-- declararea componentei
component decodor
	port(
			hex : in STD_LOGIC_VECTOR (3 downto 0); -- intrare binara
			dp : in STD_LOGIC; -- punctul zecimal
			sseg : out STD_LOGIC_VECTOR (7 downto 0)-- iesire 7 segmente
	);
end component;

signal hex_test: std_logic_vector(3 downto 0);
signal dp_test: std_logic;
signal sseg_test: std_logic_vector (7 downto 0);

begin

-- instantierea circuitului de testat uut
uut: decodor
	 port map(hex => hex_test, dp => dp_test, sseg => sseg_test);

	process
	begin
		hex_test <= "0000";
		dp_test <= '0';
		wait for 200 ns;
		loop
			-- incrementarea intrarii
			hex_test <= std_logic_vector(unsigned(hex_test) + "1");
			wait for 100 ns;
			-- la fiecare pas clipeste punctul zecimal
			dp_test <= '1';
			wait for 100 ns;
			dp_test <= '0';		
		end loop;
	end process;

end Behavioral;
