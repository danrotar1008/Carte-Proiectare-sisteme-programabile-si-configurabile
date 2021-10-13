----------------------------------------------------------------------------------
-- Company: 
-- Engineer: 
-- 
-- Create Date: 13.10.2021 10:43:13
-- Design Name: 
-- Module Name: num_sinc_TB - Behavioral
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

entity num_sinc_TB is
--  Port ( );
end num_sinc_TB;

architecture Behavioral of num_sinc_TB is

-- declararea componentei
component numarator_sincron
	generic (N: integer := 3);
	port(
		clk, reset: in std_logic;
		max_tick: out std_logic;
		q: out std_logic_vector (N-1 downto 0)
	);
end component;

-- declararea semnalelor de test
constant T: time := 100 ns;  -- perioada de ceas
signal test_clk: std_logic;
signal test_reset: std_logic;
signal test_max_tick: std_logic;
signal test_q: std_logic_vector (2 downto 0);

begin

-- instantierea circuitului de testat uut
uut: numarator_sincron
	port map(
		clk => test_clk,
    		reset => test_reset,
    		max_tick => test_max_tick,
    		q => test_q
    		);

-- ceasul 
-- ceasul de 100 ns functioneaza continuu
process
begin
	test_clk <= '0';
	wait for T/2;
	test_clk <= '1';
	wait for T/2;
end process;

-- reset
-- reset activat pentru T/2
test_reset <= '1', '0' after T/2;

end Behavioral;
