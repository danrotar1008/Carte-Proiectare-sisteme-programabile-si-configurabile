----------------------------------------------------------------------------------
-- Company: 
-- Engineer: 
-- 
-- Create Date: 13.10.2021 10:40:50
-- Design Name: 
-- Module Name: numarator_sincron - Behavioral
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

entity numarator_sincron is
generic (N: integer := 3);
port(
	clk, reset: in std_logic;
	max_tick: out std_logic;
	q: out std_logic_vector (N-1 downto 0)
	) ;
end numarator_sincron;

architecture Behavioral of numarator_sincron is

signal r_reg: unsigned(N-1 downto 0);
signal r_next: unsigned (N-1 downto 0);
begin
-- register
	process (clk, reset)
	begin
		if (reset = '1') then
			r_reg <= (others => '0');
		elsif (clk'event and clk='1') then
			r_reg <= r_next;
		end if ;
	end process;
-- next - state logic
r_next <= r_reg + 1;
-- output logic
q <= std_logic_vector(r_reg);
max_tick <= '1' when r_reg = (2**N - 1) else '0';

end Behavioral;
