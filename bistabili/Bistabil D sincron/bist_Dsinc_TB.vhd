----------------------------------------------------------------------------------
-- Company: 
-- Engineer: 
-- 
-- Create Date: 13.10.2021 03:43:50
-- Design Name: 
-- Module Name: bist_Dsinc_TB - Behavioral
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

entity bist_Dsinc_TB is
--  Port ( );
end bist_Dsinc_TB;

architecture Behavioral of bist_Dsinc_TB is

-- declararea componentei
component Bistabil_D_sincron
	port(
           clk : in STD_LOGIC;
           reset : in STD_LOGIC;
           en : in STD_LOGIC;
           d : in STD_LOGIC;
           q : out STD_LOGIC;
           nq : out STD_LOGIC);
end component;

-- declararea semnalelor de test
constant T: time := 100 ns;  -- perioada de ceas
signal test_clk: std_logic;
signal test_reset: std_logic;
signal test_en: std_logic;
signal test_d: std_logic;
signal test_q: std_logic;
signal test_nq: std_logic;

begin

-- instantierea circuitului de testat uut
uut: Bistabil_D_sincron
	port map(
		clk => test_clk,
    		reset => test_reset,
    		en => test_en,
    		d => test_d,
    		q => test_q,
    		nq => test_nq
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

-- restul stimulilor
process
begin
	test_en <= '1';
	test_d <= '0';
	wait until falling_edge(test_clk);
--	wait until rising_edge(test_clk);
	test_d <= '1';
	wait for 1.5 * T;
	test_d <= '0'; 
	wait for 1.5 * T;
end process;

end Behavioral;
