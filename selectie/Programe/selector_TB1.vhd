----------------------------------------------------------------------------------
-- Company: 
-- Engineer: 
-- 
-- Create Date: 12.10.2021 14:42:37
-- Design Name: 
-- Module Name: selector_TB - Behavioral
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

entity selector_TB is
--  Port ( );
end selector_TB;

architecture Behavioral of selector_TB is

-- declararea componentei
component selector
	port(
	intrare: in std_logic_vector(3 downto 0);
	iesire: out std_logic
	);
end component;

signal test_intrare: std_logic_vector(3 downto 0);
signal test_iesire: std_logic;

begin
-- instantierea circuitului de testat uut
uut: selector
	port map(intrare => test_intrare, iesire => test_iesire);

-- generarea vectorilor de test
	process
	begin
		wait for 100 ns;
		test_intrare <= "0000";
		wait for 100 ns;
		test_intrare <= "0001";
		wait for 100 ns;
		test_intrare <= "0010";
		wait for 100 ns;
		test_intrare <= "0011";
		wait for 100 ns;
		test_intrare <= "0100";
		wait for 100 ns;
		test_intrare <= "0101";
		wait for 100 ns;
		test_intrare <= "0110";
		wait for 100 ns;
		test_intrare <= "0111";
		wait for 100 ns;
		test_intrare <= "1000";
		wait for 100 ns;
		test_intrare <= "1001";
		wait for 100 ns;
		test_intrare <= "1010";
		wait for 100 ns;
		test_intrare <= "1011";
		wait for 100 ns;
		test_intrare <= "1100";
		wait for 100 ns;
		test_intrare <= "1101";
		wait for 100 ns;
		test_intrare <= "1110";
		wait for 100 ns;
		test_intrare <= "1111";
		wait for 100 ns;
	end process;

end Behavioral;
