-- pag (13/36) A mod-m counter counts from 0 to m - 1 and wraps around. A
-- parameterized mod-m counter is shown in Listing 4.11. It has two generics. One is M,
-- which specifies the limit, m, and the other is N, which specifies the number of bits needed
-- and should be equal to /log, M I . The code is shown in Listing 4.11, and the default value
-- is for a mod- 10 counter.

-- The baud rate generator generates a sampling signal whose frequency is exactly 16 times
-- the UART’s designated baud rate. To avoid creating a new clock domain and violating the
-- synchronous design principle, the sampling signal should function as enable ticks rather
-- than the clock signal to the UART receiver, as discussed in Section 4.3.2.
-- For the 19,200 baud rate, the sampling rate has to be 307,200 (i.e., 19,200*16) ticks per
-- second. Since the system clock rate is 50 MHz, the baud rate generator needs a mod-I63
-- (i.e., a;:$ :)-, counter, in which the one-clock-cycle tick is asserted once every 163 clock
-- cycles. The parameterized mod-m counter discussed in Section 4.3.2 can be used for this
-- purpose by setting the M generic to 163.

library ieee;
use ieee.std_logic_1164.all;
use ieee.numeric_std.all;
entity mod_m_counter is
	generic(
		N: integer := 4; -- number of bits
		M: integer := 10 -- mod-M
	);
	port (
		clk, reset: in std_logic;
		max_tick: out std_logic;
		q: out std_logic_vector (N - 1 downto 0)
	);
end mod_m_counter;

architecture arch of mod_m_counter is
	signal r_reg : unsigned (N - 1 downto 0);
	signal r_next : unsigned (N - 1 downto 0);
begin
	-- register
	process (clk, reset)
	begin
		if (reset = '1') then
			r_reg <= (others => '0');
		elsif (clk'event and clk='0') then
			r_reg <= r_next;
		end if;
	end process;
	-- next - state logic
	r_next <= (others => '0') when r_reg = (M-1) else
		r_reg + 1;
	-- output logic
	q <= std_logic_vector(r_reg);
	max_tick <= '1' when r_reg = (M - 1) else '0';
end arch;
